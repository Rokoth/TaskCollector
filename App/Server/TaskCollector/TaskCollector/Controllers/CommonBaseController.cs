using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace TaskCollector.Controllers
{
    public abstract class CommonBaseController: Controller
    {
        protected ILogger _logger;
        protected string _controllerName;

        public CommonBaseController(ILogger logger, string controllerName)
        {
            _logger = logger;
            _controllerName = controllerName;
        }

        protected IActionResult ErrorRedirect(string errorMessage, string stackTrace)
        {
            _logger.LogError($"{errorMessage} {stackTrace}");
            return RedirectToAction("Index", "Error", new { Message = errorMessage });
        }

        protected IActionResult ErrorRedirect(string action, string errorMessage, string stackTrace)
            => ErrorRedirect(GetErrorMessage(action, errorMessage), stackTrace);

        protected InternalServerErrorObjectResult InternalServerError()
            => new InternalServerErrorObjectResult();

        protected InternalServerErrorObjectResult InternalServerError(object value)
            => new InternalServerErrorObjectResult(value);

        protected string GetErrorMessage(string action, string errorMessage)
            => $"Ошибка в методе {action} контроллера {_controllerName} : {errorMessage}";

        protected Guid GetUserId()
            => Guid.Parse(User.Identity.Name);
    }
}