using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
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
        public MessageApiController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] Dictionary<string, object> message)
        {
            try
            {
                var source = new CancellationTokenSource(30000);
                var dataService = _serviceProvider.GetRequiredService<IDataService>();

                if (!User.Identity.IsAuthenticated)
                    throw new AuthenticationException();

                string userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var clientId = Guid.Parse(userClaim);

                var client = await dataService.GetClientAsync(clientId, source.Token);
                var creator = new MessageCreator
                {
                    ClientId = clientId
                };
                var mapRules = JObject.Parse(client.MapRules);
                var addFields = new Dictionary<string, object>();
                var creatorFields = typeof(MessageCreator).GetProperties();
                foreach (var item in message)
                {
                    if (item.Key.Equals("ClientId", StringComparison.InvariantCultureIgnoreCase)) continue;
                    var creatorField = creatorFields.FirstOrDefault(s => s.Name.Equals(item.Key, StringComparison.InvariantCultureIgnoreCase));
                    if (creatorField != null)
                    {
                        typeof(MessageCreator).GetProperty(creatorField.Name, System.Reflection.BindingFlags.IgnoreCase).SetValue(creator, item.Value);
                        continue;
                    }
                    addFields.Add(item.Key, item.Value);
                }
                creator.AddFields = JObject.FromObject(addFields).ToString();
                var result = await dataService.AddMessageAsync(creator, source.Token);
                return Ok(result);
            }            
            catch (Exception ex)
            {
                return BadRequest($"Ошибка при обработке сообщения: {ex.Message}");
            }
        }
    }
}
