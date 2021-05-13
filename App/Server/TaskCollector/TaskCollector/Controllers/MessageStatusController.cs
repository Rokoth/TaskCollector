///Copyright 2021 Dmitriy Rokoth
///Licensed under the Apache License, Version 2.0
///
///ref 1

using Microsoft.AspNetCore.Authorization;
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
    public class MessageStatusController : Controller
    {
        private IServiceProvider _serviceProvider;        

        public MessageStatusController(IServiceProvider serviceProvider)
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
        public async Task<ActionResult> ListPaged(Guid messageId, int page = 0, int size = 10, string sort = null, string name = null)
        {
            try
            {
                var _dataService = _serviceProvider.GetRequiredService<IGetDataService<MessageStatus, MessageStatusFilter>>();
                CancellationTokenSource source = new CancellationTokenSource(30000);
                var result = await _dataService.GetAsync(
                    new MessageStatusFilter(messageId, size, page, sort, name) , source.Token);
                var pages = result.AllCount % size == 0 ? result.AllCount / 10 : result.AllCount / 10 + 1;
                Response.Headers.Add("x-pages", pages.ToString());
                return PartialView(result.Data);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }

        ///GET: MessageController/Details/5
        [Authorize]
        public async Task<ActionResult> Details(Guid id)
        {
            try
            {
                var _dataService = _serviceProvider.GetRequiredService<IGetDataService<MessageStatus, MessageStatusFilter>>();
                var cancellationTokenSource = new CancellationTokenSource(30000);
                var result = await _dataService.GetAsync(id, cancellationTokenSource.Token);
                return View(result);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }

        // GET: MessageController/Create
        //[Authorize]
        //public ActionResult Create()
        //{
        //    //Fill default fields
        //    var message = new MessageCreator()
        //    {

        //    };
        //    return View(message);
        //}

        // POST: MessageController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize]
        //public async Task<ActionResult> Create(MessageCreator message)
        //{
        //    try
        //    {
        //        CancellationTokenSource source = new CancellationTokenSource(30000);
        //        Message result = await _dataService.AddMessageStatusAsync(message, source.Token);
        //        return RedirectToAction(nameof(Details), new { id = result.Id});
        //    }
        //    catch(Exception ex)
        //    {
        //        return RedirectToAction("Index", "Error", new { Message = ex.Message});
        //    }
        //}

        //GET: MessageController/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var _dataService = _serviceProvider.GetRequiredService<IGetDataService<MessageStatus, MessageStatusFilter>>();
                CancellationTokenSource source = new CancellationTokenSource(30000);
                var item = await _dataService.GetAsync(id, source.Token);
                //Fill fields from item
                var message = new MessageStatusUpdater()
                {

                };
                return View(message);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }

        //POST: MessageController/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, MessageStatusUpdater messageStatus)
        {
            try
            {
                var _dataService = _serviceProvider.GetRequiredService<IUpdateDataService<MessageStatus, MessageStatusUpdater>>();
                CancellationTokenSource source = new CancellationTokenSource(30000);
                MessageStatus result = await _dataService.UpdateAsync(messageStatus, source.Token);
                return RedirectToAction(nameof(Details), new { id = result.Id });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }

        // GET: MessageController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MessageController/Delete/5
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
