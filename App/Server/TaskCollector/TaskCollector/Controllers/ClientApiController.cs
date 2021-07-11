using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Common;
using TaskCollector.Contract.Model;
using TaskCollector.Service;

namespace TaskCollector.Controllers
{
    [Route("api/v1/client")]
    [ApiController]
    public class ClientApiController : ControllerBase
    {
        private IServiceProvider _serviceProvider;
        private ILogger _logger;


        public ClientApiController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = _serviceProvider.GetRequiredService<ILogger<ClientApiController>>();
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Auth([FromBody] Contract.Model.ClientIdentity login)
        {
            try
            {
                var source = new CancellationTokenSource(30000);
                var dataService = _serviceProvider.GetRequiredService<IAuthService>();

                var identity = await dataService.Auth(login, source.Token);
                if (identity == null)
                {
                    return BadRequest(new { errorText = "Invalid username or password." });
                }
                
                var now = DateTime.UtcNow;
                // создаем JWT-токен
                var jwt = new JwtSecurityToken(
                        issuer: AuthOptions.ISSUER,
                        audience: AuthOptions.AUDIENCE,
                        notBefore: now,
                        claims: identity.Claims,
                        expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                var response = new ClientIdentityResponse
                {
                    Token = encodedJwt,
                    UserName = identity.Name
                };

                return Ok(response);
            }            
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при обработке запроса: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Ошибка при обработке запроса: {ex.Message}");
            }
        }        
    }
}
