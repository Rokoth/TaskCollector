using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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

using TaskCollector.Service;

namespace TaskCollector.Controllers
{
    [Route("api/v1/client")]
    [ApiController]
    public class ClientApiController : ControllerBase
    {
        private IServiceProvider _serviceProvider;
        private readonly SignInManager<Db.Model.ClientIdentity> _signInManager;

        public ClientApiController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _signInManager = serviceProvider.GetRequiredService<SignInManager<Db.Model.ClientIdentity>>();
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Auth([FromBody] Contract.Model.ClientIdentity login)
        {
            try
            {
                var source = new CancellationTokenSource(30000);
                var dataService = _serviceProvider.GetRequiredService<IDataService>();

                
                return Ok();
            }            
            catch (Exception ex)
            {
                return BadRequest($"Ошибка при обработке запроса: {ex.Message}");
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {               
                if (!User.Identity.IsAuthenticated)
                    throw new AuthenticationException();
                await _signInManager.SignOutAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка при обработке запроса: {ex.Message}");
            }
        }
    }
}
