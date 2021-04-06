///Copyright 2021 Dmitriy Rokoth
///Licensed under the Apache License, Version 2.0
///
///ref 1

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Contract.Model;
using TaskCollector.Service;

namespace TaskCollector.Controllers
{
    public class MessageController : Controller
    {
        private IServiceProvider _serviceProvider;
        private IDataService _dataService;

        public MessageController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _dataService = _serviceProvider.GetRequiredService<IDataService>();
        }

        // GET: MessageController
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ListPaged(int page = 0, int size = 10, string sort = null, string name = null)
        {
            try
            {
                CancellationTokenSource source = new CancellationTokenSource(30000);
                var result = await _dataService.GetMessagesAsync(new MessageFilter(size, page, sort, name) , source.Token);
                return PartialView(result);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }

        // GET: MessageController/Details/5
        public async Task<ActionResult> Details(Guid id)
        {
            try
            {
                var cancellationTokenSource = new CancellationTokenSource(30000);
                Message result = await _dataService.GetMessageAsync(id, cancellationTokenSource.Token);
                return View(result);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }

        // GET: MessageController/Create
        public ActionResult Create()
        {
            //Fill default fields
            var message = new MessageCreator()
            {

            };
            return View(message);
        }

        // POST: MessageController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MessageCreator message)
        {
            try
            {
                CancellationTokenSource source = new CancellationTokenSource(30000);
                Message result = await _dataService.AddMessage(message, source.Token);
                return RedirectToAction(nameof(Details), new { id = result.Id});
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index", "Error", new { Message = ex.Message});
            }
        }

        // GET: MessageController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MessageController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: MessageController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MessageController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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
