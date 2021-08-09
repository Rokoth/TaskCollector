///Copyright 2021 Dmitriy Rokoth
///Licensed under the Apache License, Version 2.0
///
///ref 1
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Contract.Model;
using TaskCollector.Service;

namespace TaskCollector.Controllers
{
    public class ClientController : Controller
    {
        private IServiceProvider _serviceProvider;
        private ILogger _logger;

        public ClientController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = _serviceProvider.GetRequiredService<ILogger<ClientController>>();
        }

        // GET: ClientController
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<ActionResult> ListPaged(int page = 0, int size = 10, string sort = null, string name = null, string login = null)
        {
            try
            {
                var userId = Guid.Parse(User.Identity.Name);
                var _dataService = _serviceProvider.GetRequiredService<IGetDataService<Client, ClientFilter>>();
                CancellationTokenSource source = new CancellationTokenSource(30000);
                var result = await _dataService.GetAsync(new ClientFilter(size, page, sort, name, login, userId), source.Token);
                var pages = result.AllCount % size == 0 ? result.AllCount / size : result.AllCount / size + 1;
                Response.Headers.Add("x-pages", pages.ToString());
                return PartialView(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка в методе ListPaged : {ex.Message} {ex.StackTrace}");
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }

        // GET: UserController
        [Authorize]
        public ActionResult History()
        {
            return View();
        }

        [Authorize]
        public async Task<ActionResult> HistoryListPaged(int page = 0, int size = 10, string sort = null, string name = null, Guid? id = null)
        {
            try
            {
                var _dataService = _serviceProvider.GetRequiredService<IGetDataService<ClientHistory, ClientHistoryFilter>>();
                CancellationTokenSource source = new CancellationTokenSource(30000);
                var result = await _dataService.GetAsync(new ClientHistoryFilter(size, page, sort, name, id), source.Token);
                Response.Headers.Add("x-pages", result.AllCount.ToString());
                return PartialView(result.Data);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }

        // GET: ClientController/Details/5
        [Authorize]
        public async Task<ActionResult> Details(Guid id)
        {
            try
            {
                var userId = Guid.Parse(User.Identity.Name);
                var _dataService = _serviceProvider.GetRequiredService<IGetDataService<Client, ClientFilter>>();
                var cancellationTokenSource = new CancellationTokenSource(30000);
                Client result = await _dataService.GetAsync(id, cancellationTokenSource.Token);
                if(result.UserId == userId)
                    return View(result);
                return RedirectToAction("Index", "Error", new { Message = "Клиент привязан к другому пользователю" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка в методе Details : {ex.Message} {ex.StackTrace}");
                return RedirectToAction("Index", "Error", new { Message = ex.Message});
            }
        }

        // GET: ClientController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(ClientCreator creator)
        {
            try
            {
                var userId = Guid.Parse(User.Identity.Name);
                creator.UserId = userId;
                var _dataService = _serviceProvider.GetRequiredService<IAddDataService<Client, ClientCreator>>();
                var cancellationTokenSource = new CancellationTokenSource(30000);
                Client result = await _dataService.AddAsync(creator, cancellationTokenSource.Token);
                return RedirectToAction(nameof(Details), new { id = result.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка в методе Create : {ex.Message} {ex.StackTrace}");
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<JsonResult> CheckName(string name)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(name))
            {
                var _dataService = _serviceProvider.GetRequiredService<IGetDataService<Client, ClientFilter>>();
                var cancellationTokenSource = new CancellationTokenSource(30000);
                var check = await _dataService.GetAsync(new ClientFilter(10,0, null, name, null, null), cancellationTokenSource.Token);
                result = !check.Data.Any();
            }
            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> CheckLogin(string login)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(login))
            {
                var _dataService = _serviceProvider.GetRequiredService<IGetDataService<Client, ClientFilter>>();
                var cancellationTokenSource = new CancellationTokenSource(30000);
                var check = await _dataService.GetAsync(new ClientFilter(10, 0, null, null, login, null), cancellationTokenSource.Token);
                result = !check.Data.Any();
            }
            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> CheckNameEdit(string name, Guid id)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(name))
            {
                var _dataService = _serviceProvider.GetRequiredService<IGetDataService<Client, ClientFilter>>();
                var cancellationTokenSource = new CancellationTokenSource(30000);
                var check = await _dataService.GetAsync(new ClientFilter(10, 0, null, name, null, null), cancellationTokenSource.Token);
                result = !check.Data.Where(s=>s.Name == name && s.Id!=id).Any();
            }
            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> CheckLoginEdit(string login, Guid id)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(login))
            {
                var _dataService = _serviceProvider.GetRequiredService<IGetDataService<Client, ClientFilter>>();
                var cancellationTokenSource = new CancellationTokenSource(30000);
                var check = await _dataService.GetAsync(new ClientFilter(10, 0, null, null, login, null), cancellationTokenSource.Token);
                result = !check.Data.Where(s => s.Login == login && s.Id != id).Any();
            }
            return Json(result);
        }

        // GET: ClientController/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var userId = Guid.Parse(User.Identity.Name);
                var _dataService = _serviceProvider.GetRequiredService<IGetDataService<Client, ClientFilter>>();
                CancellationTokenSource source = new CancellationTokenSource(30000);
                Client result = await _dataService.GetAsync(id, source.Token);
                if (result.UserId == userId)
                {
                    var updater = new ClientUpdater()
                    {
                        Description = result.Description,
                        Id = result.Id,
                        Login = result.Login,
                        Name = result.Name,
                        PasswordChanged = false,
                        UserId = result.UserId
                    };
                    return View(updater);
                }
                
                return RedirectToAction("Index", "Error", new { Message = "Клиент привязан к другому пользователю" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка в методе Edit : {ex.Message} {ex.StackTrace}");
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }

        // POST: ClientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id, ClientUpdater updater)
        {
            try
            {
                var userId = Guid.Parse(User.Identity.Name);
                var _getDataService = _serviceProvider.GetRequiredService<IGetDataService<Client, ClientFilter>>();
                var _dataService = _serviceProvider.GetRequiredService<IUpdateDataService<Client, ClientUpdater>>();
                CancellationTokenSource source = new CancellationTokenSource(30000);
                Client check = await _getDataService.GetAsync(id, source.Token);
                if (check.UserId == userId)
                {
                    Client result = await _dataService.UpdateAsync(updater, source.Token);
                    return RedirectToAction(nameof(Details), new { id = result.Id });
                }
                return RedirectToAction("Index", "Error", new { Message = "Клиент привязан к другому пользователю" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка в методе Edit : {ex.Message} {ex.StackTrace}");
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }

        // GET: ClientController/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var userId = Guid.Parse(User.Identity.Name);
                var _dataService = _serviceProvider.GetRequiredService<IGetDataService<Client, ClientFilter>>();
                CancellationTokenSource source = new CancellationTokenSource(30000);
                Client result = await _dataService.GetAsync(id, source.Token);
                if (result.UserId == userId)
                {
                    return View(result);
                }
                return RedirectToAction("Index", "Error", new { Message = "Клиент привязан к другому пользователю" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка в методе Delete : {ex.Message} {ex.StackTrace}");
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }

        // POST: ClientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id, User model)
        {
            try
            {
                var userId = Guid.Parse(User.Identity.Name);
                var _getDataService = _serviceProvider.GetRequiredService<IGetDataService<Client, ClientFilter>>();
                var _dataService = _serviceProvider.GetRequiredService<IDeleteDataService<Client>>();
                CancellationTokenSource source = new CancellationTokenSource(30000);
                Client check = await _getDataService.GetAsync(id, source.Token);
                if (check.UserId == userId)
                {
                    Client result = await _dataService.DeleteAsync(id, source.Token);
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction("Index", "Error", new { Message = "Клиент привязан к другому пользователю" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка в методе Delete : {ex.Message} {ex.StackTrace}");
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }
    }
}
