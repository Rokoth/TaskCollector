///Copyright 2021 Dmitriy Rokoth
///Licensed under the Apache License, Version 2.0
///
///ref 1
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
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

        public ClientController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;                   
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
                var pages = result.AllCount % size == 0 ? result.AllCount / 10 : result.AllCount / 10 + 1;
                Response.Headers.Add("x-pages", pages.ToString());
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
                var _dataService = _serviceProvider.GetRequiredService<IGetDataService<Client, ClientFilter>>();
                var cancellationTokenSource = new CancellationTokenSource(30000);
                Client result = await _dataService.GetAsync(id, cancellationTokenSource.Token);
                return View(result);
            }
            catch (Exception ex)
            {
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
                var _dataService = _serviceProvider.GetRequiredService<IAddDataService<Client, ClientCreator>>();
                var cancellationTokenSource = new CancellationTokenSource(30000);
                Client result = await _dataService.AddAsync(creator, cancellationTokenSource.Token);
                return View(result);
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

        // GET: ClientController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ClientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClientController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ClientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
