///Copyright 2021 Dmitriy Rokoth
///Licensed under the Apache License, Version 2.0
///
///ref 1
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TaskCollector.Contract.Model;
using TaskCollector.Models;

namespace TaskCollector.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index([FromQuery]string message, [FromQuery] string source = null)
        {
            return View(new ErrorMessage()
            { 
               Message = message,
               Source = source
            });
        }        
    }
}
