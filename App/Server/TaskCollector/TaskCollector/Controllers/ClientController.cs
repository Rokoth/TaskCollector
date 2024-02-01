///Copyright 2021 Dmitriy Rokoth
///Licensed under the Apache License, Version 2.0
///
///ref 2
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TaskCollector.Contract.Model;
using TaskCollector.Service;

namespace TaskCollector.Controllers
{
    /// <summary>
    /// Контроллер для работы с моделью Client
    /// </summary>
    public class ClientController : BaseController<Client, ClientFilter, ClientHistory, ClientHistoryFilter, ClientCreator, ClientUpdater>
    {                
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="getDataService"></param>
        /// <param name="getHistoryDataService"></param>
        /// <param name="addDataService"></param>
        /// <param name="updateDataService"></param>
        /// <param name="deleteDataService"></param>
        /// <param name="mapper"></param>
        public ClientController(
            ILogger<ClientController> logger,
            IGetDataService<Client, ClientFilter> getDataService,
            IGetDataService<ClientHistory, ClientHistoryFilter> getHistoryDataService,
            IAddDataService<Client, ClientCreator> addDataService,
            IUpdateDataService<Client, ClientUpdater> updateDataService,
            IDeleteDataService<Client> deleteDataService,
            IMapper mapper) 
            : base(
                  logger,
                  getDataService,
                  getHistoryDataService,
                  addDataService,
                  updateDataService,
                  deleteDataService, 
                  mapper, 
                  nameof(ClientController))
        {                        
            
        }                

        /// <summary>
        /// Метод проверки на существование клиента с повторяющимся именем
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CheckName(string name, Guid? id = null) 
            => await ExecuteCheck(name, ClientFilter.NameClientFilter, id, nameof(CheckName));

        /// <summary>
        /// Метод проверки на существование клиента с повторяющимся логином
        /// </summary>
        /// <param name="login"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CheckLogin(string login, Guid? id = null)
            => await ExecuteCheck(login, ClientFilter.LoginClientFilter, id, nameof(CheckLogin));
        
    }
}
