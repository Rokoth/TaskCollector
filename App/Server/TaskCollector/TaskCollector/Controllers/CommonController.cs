//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref 2
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TaskCollector.Deploy;

namespace TaskCollector.Controllers
{
    /// <summary>
    ///Общий контроллер, содержащий методы, не привязанные к какой-либо бизнес-модели
    /// </summary>
    [Produces("application/json")]
    [Route("/api/v1/common")]
    public class CommonController : CommonBaseController
    {
        private readonly IDeployService _deployService;
        private const string _dbDeployErrorTemplate = "Ошибка при раскатке базы данных";

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="deployService"></param>
        public CommonController(ILogger<CommonController> logger, IDeployService deployService ) : base(logger, nameof(CommonController))
        {            
            _deployService = deployService;
        }

        /// <summary>
        /// Проверка доступности сервиса
        /// </summary>
        /// <returns></returns>
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("pong");
        }

        /// <summary>
        /// Обновление БД
        /// </summary>
        /// <returns></returns>
        [HttpGet("deploy")]
        public async Task<IActionResult> Deploy()
        {
            try
            {
                await _deployService.Deploy();
                return Ok("База данных обновлена");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{_dbDeployErrorTemplate}: {ex.Message}\r\n{ex.StackTrace}");
                return InternalServerError($"{_dbDeployErrorTemplate}: {ex.Message}");
            }
        }        
    }
}
