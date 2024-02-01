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
    public class UserController : Controller
    {
        private IServiceProvider _serviceProvider;
        private ILogger _logger;

        public UserController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = _serviceProvider.GetRequiredService<ILogger<UserController>>();
        }

        // GET: UserController
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
                var _dataService = _serviceProvider.GetRequiredService<IGetDataService<User, UserFilter>>();
                CancellationTokenSource source = new CancellationTokenSource(30000);
                var result = await _dataService.GetAsync(new UserFilter(size, page, sort, name, login), GetUserId(), source.Token);
                var pages = (result.AllCount % size == 0) ? (result.AllCount / size) : ((result.AllCount / size) + 1);
                Response.Headers.Add("x-pages", pages.ToString());
                return PartialView(result.Data);
            }
            catch (Exception ex)
            {
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
                var _dataService = _serviceProvider.GetRequiredService<IGetDataService<UserHistory, UserHistoryFilter>>();
                CancellationTokenSource source = new CancellationTokenSource(30000);
                var result = await _dataService.GetAsync(new UserHistoryFilter(size, page, sort, name, id), GetUserId(), source.Token);
                Response.Headers.Add("x-pages", result.AllCount.ToString());
                return PartialView(result.Data);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }

        // GET: UserController/Details/5
        [Authorize]
        public async Task<ActionResult> Details(Guid id)
        {
            try
            {
                var _dataService = _serviceProvider.GetRequiredService<IGetDataService<User, UserFilter>>();
                var cancellationTokenSource = new CancellationTokenSource(30000);
                User result = await _dataService.GetAsync(id, GetUserId(), cancellationTokenSource.Token);
                return View(result);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }

        // GET: UserController/Create
        [Authorize]
        public ActionResult Create()
        {
            //Fill default fields
            var user = new UserCreator()
            {
                
            };
            return View(user);
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create(UserCreator creator)
        {
            try
            {
                var _dataService = _serviceProvider.GetRequiredService<IAddDataService<User, UserCreator>>();
                CancellationTokenSource source = new CancellationTokenSource(30000);
                User result = await _dataService.AddAsync(creator, GetUserId(), source.Token);
                return RedirectToAction(nameof(Details), new { id = result.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка в методе UserController::Create :{ex.Message} {ex.StackTrace}");
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }

        // GET: UserController/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var _dataService = _serviceProvider.GetRequiredService<IGetDataService<User, UserFilter>>();
                CancellationTokenSource source = new CancellationTokenSource(30000);
                User result = await _dataService.GetAsync(id, GetUserId(), source.Token);
                var updater = new UserUpdater()
                {
                    Description = result.Description,
                    Id = result.Id,
                    Login = result.Login,
                    Name = result.Name,
                    PasswordChanged = false
                };
                return View(updater);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<JsonResult> CheckName(string name)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(name))
            {
                var _dataService = _serviceProvider.GetRequiredService<IGetDataService<User, UserFilter>>();
                var cancellationTokenSource = new CancellationTokenSource(30000);
                var check = await _dataService.GetAsync(new UserFilter(10, 0, null, name, null), GetUserId(), cancellationTokenSource.Token);
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
                var _dataService = _serviceProvider.GetRequiredService<IGetDataService<User, UserFilter>>();
                var cancellationTokenSource = new CancellationTokenSource(30000);
                var check = await _dataService.GetAsync(new UserFilter(10, 0, null, null, login), GetUserId(), cancellationTokenSource.Token);
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
                var _dataService = _serviceProvider.GetRequiredService<IGetDataService<User, UserFilter>>();
                var cancellationTokenSource = new CancellationTokenSource(30000);
                var check = await _dataService.GetAsync(new UserFilter(10, 0, null, name, null), GetUserId(), cancellationTokenSource.Token);
                result = !check.Data.Where(s => s.Name == name && s.Id != id).Any();
            }
            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> CheckLoginEdit(string login, Guid id)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(login))
            {
                var _dataService = _serviceProvider.GetRequiredService<IGetDataService<User, UserFilter>>();
                var cancellationTokenSource = new CancellationTokenSource(30000);
                var check = await _dataService.GetAsync(new UserFilter(10, 0, null, null, login), GetUserId(), cancellationTokenSource.Token);
                result = !check.Data.Where(s => s.Login == login && s.Id != id).Any();
            }
            return Json(result);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Edit(Guid id, UserUpdater updater)
        {
            try
            {
                var _dataService = _serviceProvider.GetRequiredService<IUpdateDataService<User, UserUpdater>>();
                CancellationTokenSource source = new CancellationTokenSource(30000);
                User result = await _dataService.UpdateAsync(updater, GetUserId(), source.Token);
                return RedirectToAction(nameof(Details), new { id = result.Id });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }

        // GET: UserController/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var _dataService = _serviceProvider.GetRequiredService<IGetDataService<User, UserFilter>>();
                CancellationTokenSource source = new CancellationTokenSource(30000);
                User result = await _dataService.GetAsync(id, GetUserId(), source.Token);
                if(result == null)
                    return RedirectToAction("Index", "Error", new { Message = "Пользователь не найден" });

                return View(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка в методе Delete : {ex.Message} {ex.StackTrace}");
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Delete(Guid id, User model)
        {
            try
            {
                var _dataService = _serviceProvider.GetRequiredService<IDeleteDataService<User>>();
                CancellationTokenSource source = new CancellationTokenSource(30000);
                User result = await _dataService.DeleteAsync(id, GetUserId(), source.Token);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }

        protected Guid GetUserId() => Guid.Parse(User.Identity.Name);
    }
}
