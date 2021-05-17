///Copyright 2021 Dmitriy Rokoth
///Licensed under the Apache License, Version 2.0
///
///ref 1

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Contract.Model;
using TaskCollector.Service;

namespace TaskCollector.Controllers
{
    public class MessageController : Controller
    {
        private IServiceProvider _serviceProvider;       

        public MessageController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // GET: MessageController
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<ActionResult> ListPaged(int page = 0, int size = 10, 
            string sort = null, string title = null, Guid? clientId = null, List<int> levels  = null, DateTimeOffset? from = null, DateTimeOffset? to = null)
        {
            try
            {
                var _dataService = _serviceProvider.GetRequiredService<IGetDataService<Message, MessageFilter>>();
                CancellationTokenSource source = new CancellationTokenSource(30000);
                var result = await _dataService.GetAsync(new MessageFilter(size, page, sort, title, clientId, levels, from, to) , source.Token);
                var pages = result.AllCount % size == 0 ? result.AllCount / 10 : result.AllCount / 10 + 1;
                Response.Headers.Add("x-pages", pages.ToString());
                return PartialView(result.Data);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }

        // GET: MessageController
        [Authorize]
        public ActionResult History()
        {
            return View();
        }

        [Authorize]
        public async Task<ActionResult> HistoryListPaged(int page = 0, int size = 10,
            string sort = null, string title = null, Guid? clientId = null, Guid? id = null, DateTimeOffset? from = null, DateTimeOffset? to = null)
        {
            try
            {
                var _dataService = _serviceProvider.GetRequiredService<IGetDataService<MessageHistory, MessageHistoryFilter>>();
                CancellationTokenSource source = new CancellationTokenSource(30000);
                var result = await _dataService.GetAsync(new MessageHistoryFilter(size, page, sort, title, id, clientId, from, to), source.Token);
                var pages = result.AllCount % size == 0 ? result.AllCount / 10 : result.AllCount / 10 + 1;
                Response.Headers.Add("x-pages", pages.ToString());
                return PartialView(result.Data);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }

        [Authorize]
        // GET: MessageController/Details/5
        public async Task<ActionResult> Details(Guid id)
        {
            try
            {
                var _dataService = _serviceProvider.GetRequiredService<IGetDataService<Message, MessageFilter>>();
                var cancellationTokenSource = new CancellationTokenSource(30000);
                Message result = await _dataService.GetAsync(id, cancellationTokenSource.Token);
                return View(result);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }

        // GET: MessageController/Create
        [Authorize]
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
        [Authorize]
        public async Task<ActionResult> Create(MessageCreator message)
        {
            try
            {
                var _dataService = _serviceProvider.GetRequiredService<IAddDataService<Message, MessageCreator>>();
                CancellationTokenSource source = new CancellationTokenSource(30000);
                Message result = await _dataService.AddAsync(message, source.Token);
                return RedirectToAction(nameof(Details), new { id = result.Id});
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index", "Error", new { Message = ex.Message});
            }
        }

        // GET: MessageController/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var _dataService = _serviceProvider.GetRequiredService<IGetDataService<Message, MessageFilter>>();
                CancellationTokenSource source = new CancellationTokenSource(30000);
                var item = await _dataService.GetAsync(id, source.Token);
                //Fill fields from item
                var message = new MessageUpdater()
                {

                };
                return View(message);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }

        // POST: MessageController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id, MessageUpdater message)
        {
            try
            {
                var _dataService = _serviceProvider.GetRequiredService<IUpdateDataService<Message, MessageUpdater>>();
                CancellationTokenSource source = new CancellationTokenSource(30000);
                Message result = await _dataService.UpdateAsync(message, source.Token);
                return RedirectToAction(nameof(Details), new { id = result.Id });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }

        // GET: MessageController/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var _dataService = _serviceProvider.GetRequiredService<IGetDataService<Message, MessageFilter>>();
                CancellationTokenSource source = new CancellationTokenSource(30000);
                var item = await _dataService.GetAsync(id, source.Token);                
                return View(item);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }

        // POST: MessageController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id, Message message)
        {
            try
            {
                var _dataService = _serviceProvider.GetRequiredService<IDeleteDataService<Message>>();
                CancellationTokenSource source = new CancellationTokenSource(30000);
                Message result = await _dataService.DeleteAsync(id, source.Token);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }
    }
}
