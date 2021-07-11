﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskCollector.Deploy;

namespace TaskCollector.Controllers
{
    [Produces("application/json")]
    [Route("/api/v1/common")]
    public class CommonController : CommonControllerBase
    {
        private readonly IDeployService deployService;

        public CommonController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _logger = serviceProvider.GetRequiredService<ILogger<CommonController>>();
            deployService = serviceProvider.GetRequiredService<IDeployService>();
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok();
        }

        [HttpGet("deploy")]
        public async Task<IActionResult> Deploy()
        {
            try
            {
                await deployService.Deploy();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при раскатке базы данных: {ex.Message} {ex.StackTrace}");
                return InternalServerError($"Ошибка при раскатке базы данных: {ex.Message}");
            }
        }        
    }

    public abstract class CommonControllerBase : Controller
    {
        protected ILogger _logger;
        protected IServiceProvider _serviceProvider;

        public CommonControllerBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = serviceProvider.GetRequiredService<ILogger<CommonControllerBase>>();
        }

        protected InternalServerErrorObjectResult InternalServerError()
        {
            return new InternalServerErrorObjectResult();
        }

        protected InternalServerErrorObjectResult InternalServerError(object value)
        {
            return new InternalServerErrorObjectResult(value);
        }

        protected IActionResult ErrorRedirect(string errorMessage, string stackTrace)
        {
            _logger.LogError($"{errorMessage} {stackTrace}");
            return RedirectToAction("Index", "Error", new { Message = errorMessage });
        }
    }

    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object value) : base(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }

        public InternalServerErrorObjectResult() : this(null)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
