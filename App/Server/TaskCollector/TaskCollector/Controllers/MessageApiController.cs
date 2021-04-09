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
                var creator = new MessageCreator();
                creator.ClientId = clientId;
                var mapRules = JObject.Parse(client.MapRules);
                foreach (var item in message)
                {
                    if (item.Key == "ClientId") continue;
                      
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка при обработке сообщения: {ex.Message}");
            }
        }
    }
}
