using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json.Linq;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Contract.Model;
using TaskCollector.Service;

namespace TaskCollector.Controllers
{
    [Route("api/v1/message")]
    [ApiController]
    public class MessageApiController : ControllerBase
    {
        private IServiceProvider _serviceProvider;
        private ILogger _logger;

        public MessageApiController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = _serviceProvider.GetRequiredService<ILogger<MessageApiController>>();
        }

        [HttpPost("send")]
        [Authorize("Token")]
        public async Task<IActionResult> Send([FromBody] Dictionary<string, object> message)
        {
            try
            {
                var source = new CancellationTokenSource(30000);
                var clientDataService = _serviceProvider.GetRequiredService<IGetDataService<Client, ClientFilter>>();
                var messageDataService = _serviceProvider.GetRequiredService<IAddDataService<Message, MessageCreator>>();

                if (!User.Identity.IsAuthenticated)
                {
                    return Unauthorized($"{Request.Scheme}://{Request.Host.Value}/api/v1/client/auth");
                }

                string userClaim = User.Identity.Name;
                    //User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var clientId = Guid.Parse(userClaim);

                var client = await clientDataService.GetAsync(clientId, source.Token);
                var creator = new MessageCreator
                {
                    ClientId = clientId,
                    CreatedDate = DateTimeOffset.Now
                };
                var mapRules = client.MapRules != null ? JObject.Parse(client.MapRules) : null;
                var addFields = new Dictionary<string, object>();
                var creatorFields = typeof(MessageCreator).GetProperties();
               
                foreach (var item in message)
                {
                    _logger.LogDebug($"Установка поля {item.Key} : {item.Value}");
                    try
                    {
                        if (item.Key.Equals("ClientId", StringComparison.InvariantCultureIgnoreCase)) continue;
                        var creatorField = creatorFields.FirstOrDefault(s => s.Name.Equals(item.Key, StringComparison.InvariantCultureIgnoreCase));
                        if (creatorField != null)
                        {
                            var propType = ReflectionHelper.IsNullableType(creatorField.PropertyType)
                               ? Nullable.GetUnderlyingType(creatorField.PropertyType)
                               : creatorField.PropertyType;

                            var itemValue = ((JsonElement)(item.Value)).ToObject(propType);
                            creatorField.SetValue(creator, itemValue);                            
                            
                            continue;
                        }
                        addFields.Add(item.Key, ((JsonElement)(item.Value)).GetString());
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Ошибка при установке поля {item.Key} : {item.Value} : {ex.Message} {ex.StackTrace}");
                        throw;
                    }
                }
                creator.AddFields = JObject.FromObject(addFields).ToString();
                if (string.IsNullOrEmpty(creator.FeedbackContact))
                    creator.FeedbackContact = "None";
                var result = await messageDataService.AddAsync(creator, source.Token);                
                return Ok(result);
            }            
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при обработке сообщения: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Ошибка при обработке сообщения: {ex.Message}");
            }
        }
    }

    public static class ReflectionHelper
    {
        public static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }

    public static partial class JsonExtensions
    {
        public static object ToObject(this JsonElement element, Type type, JsonSerializerOptions options = null)
        {
            var bufferWriter = new ArrayBufferWriter<byte>();
            using (var writer = new Utf8JsonWriter(bufferWriter))
                element.WriteTo(writer);
            
            return JsonSerializer.Deserialize(bufferWriter.WrittenSpan, type, options);
        }
    }
}
