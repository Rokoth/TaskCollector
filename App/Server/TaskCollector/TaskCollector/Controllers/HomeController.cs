///Copyright 2021 Dmitriy Rokoth
///Licensed under the Apache License, Version 2.0
///
///ref 1
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TaskCollector.Deploy;
using TaskCollector.Models;

namespace TaskCollector.Controllers
{
    public class HomeController : CommonControllerBase
    {        
        private readonly IDeployService _deployService;

        public HomeController(IServiceProvider provider, IDeployService deployService): base(provider)
        {            
            _deployService = deployService;
            _logger = provider.GetRequiredService<ILogger<HomeController>>();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
               
        public async Task<IActionResult> Deploy()
        {
            try
            {
                await _deployService.Deploy();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при раскатке базы данных: {ex.Message} {ex.StackTrace}");
                return InternalServerError($"Ошибка при раскатке базы данных: {ex.Message}");
            }
        }
    }
}
