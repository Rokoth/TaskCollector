using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq.Dynamic.Core;
using TaskCollector.Contract.Model;
using TaskCollector.Service;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace TaskCollector.Controllers
{
    public abstract class BaseController<Tdto, TFilter, THdto, THFilter, TCreator, TUpdater> : CommonBaseController 
        where Tdto : Entity 
        where TFilter : Filter<Tdto>
        where THdto : EntityHistory
        where THFilter : Filter<THdto>
        where TUpdater: IEntity, new()
    {
       
        protected const int _defaultCancellationTokenTime = 30000;
       
        protected readonly IGetDataService<Tdto, TFilter> _getDataService;
        protected readonly IGetDataService<THdto, THFilter> _getHistoryDataService;
        protected readonly IAddDataService<Tdto, TCreator> _addDataService;
        protected readonly IUpdateDataService<Tdto, TUpdater> _updateDataService;
        protected readonly IDeleteDataService<Tdto> _deleteDataService;
        protected readonly IMapper _mapper;

        public BaseController(
            ILogger<BaseController<Tdto, TFilter, THdto, THFilter, TCreator, TUpdater>> logger, 
            IGetDataService<Tdto, TFilter> getDataService,
            IGetDataService<THdto, THFilter> getHistoryDataService,
            IAddDataService<Tdto, TCreator> addDataService,
            IUpdateDataService<Tdto, TUpdater> updateDataService,
            IDeleteDataService<Tdto> deleteDataService,
            IMapper mapper,
            string controllerName) : base(logger, controllerName)
        {                       
            _getDataService = getDataService;
            _getHistoryDataService = getHistoryDataService;
            _addDataService = addDataService;
            _updateDataService = updateDataService;
            _deleteDataService = deleteDataService;
            _mapper = mapper;
        }

        [Authorize]
        public IActionResult Index()
            => View();

        [Authorize]
        public async Task<IActionResult> ListPaged([FromQuery] TFilter filter)
        {
            return await ExecutePagedWithTry((userId, token) =>
                _getDataService.GetAsync(filter, userId, token),
                nameof(ListPaged),
                filter.Size);
        }

        [Authorize]
        public IActionResult History()
            => View();

        [Authorize]
        public async Task<IActionResult> HistoryListPaged([FromQuery] THFilter filter)
        {
            return await ExecutePagedWithTry((userId, token) =>
                _getHistoryDataService.GetAsync(filter, userId, token),
                nameof(HistoryListPaged),
                filter.Size);
        }

        [Authorize]
        public async Task<IActionResult> Details([FromQuery] Guid id)
        {
            return await ExecuteWithTry(async (userId, token) =>
            {
                var result = await _getDataService.GetAsync(id, userId, token);
                return View(result);
            },
            nameof(Details));
        }

        [Authorize]
        public ActionResult Create()
            => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(TCreator creator)
        {
            return await ExecuteWithTry(async (userId, token) =>
            {
                var result = await _addDataService.AddAsync(creator, userId, token);
                return RedirectToAction(nameof(Details), new { id = result.Id });
            },
            nameof(Create));
        }

        [Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
            return await ExecuteWithTry(async (userId, token) =>
            {
                var result = await _getDataService.GetAsync(id, userId, token);
                return View(_mapper.Map<TUpdater>(result));
            },
            nameof(Edit));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(TUpdater updater)
        {
            return await ExecuteWithTry(async (userId, token) =>
            {
                var result = await _updateDataService.UpdateAsync(updater, userId, token);
                return RedirectToAction(nameof(Details), new { id = result.Id });
            },
            nameof(Edit));
        }

        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await ExecuteWithTry(async (userId, token) =>
            {
                var result = await _getDataService.GetAsync(id, userId, token);
                return View(result);
            },
            nameof(Delete));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id, Tdto model)
        {
            return await ExecuteWithTry(async (userId, token) =>
            {
                var result = await _deleteDataService.DeleteAsync(id, userId, token);
                return RedirectToAction(nameof(Index));
            },
            nameof(Delete));
        }

        

        protected Task<IActionResult> ExecutePagedWithTry<T>(
            Func<Guid, CancellationToken, Task<Contract.Model.PagedResult<T>>> action,
            string actionName,
            int pageSize)
            => ExecuteWithTry((userId, token) => GetPaged(action, actionName, pageSize, userId, token), actionName);

        private async Task<IActionResult> GetPaged<T>(
            Func<Guid, CancellationToken, Task<Contract.Model.PagedResult<T>>> action, 
            string actionName, 
            int pageSize,
            Guid userId,
            CancellationToken token)
        {
            var result = await action(userId, token);
            Response.Headers.Add("x-pages", GetPagesCount(result.AllCount, pageSize).ToString());
            return PartialView(actionName, result.Data);
        }

        protected async Task<IActionResult> ExecuteWithTry(
            Func<Guid, CancellationToken, Task<IActionResult>> action,            
            string actionName)
        {
            try
            {                
                return await action(GetUserId(), CreateCancellationToken());
            }
            catch (Exception ex)
            {
                return ErrorRedirect(actionName, ex.Message, ex.StackTrace);
            }
        }

        protected async Task<IActionResult> ExecuteWithTryThrow(
            Func<Guid, CancellationToken, Task<IActionResult>> action,            
            string actionName)
        {
            try
            {
                return await action(GetUserId(), CreateCancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(GetErrorMessage(actionName, ex.Message));
                throw ex;
            }
        }

        protected static int GetPagesCount(int allCount, int size)
            => allCount % size == 0 ? allCount / size : allCount / size + 1;

        protected CancellationToken CreateCancellationToken() 
            => new CancellationTokenSource(_defaultCancellationTokenTime).Token;

        protected async Task<IActionResult> ExecuteCheck(string value, Func<string, TFilter> createFilter, Guid? id, string actionName)
            => await ExecuteWithTryThrow((userId, token) => ExecuteCheck(value, createFilter, id, userId, token), actionName);

        private async Task<IActionResult> ExecuteCheck(
            string value, 
            Func<string, TFilter> createFilter,
            Guid? id,
            Guid userId,
            CancellationToken token)
        {
            if (string.IsNullOrEmpty(value))
                return Json(false);

            var check = await _getDataService.GetAsync(createFilter(value), userId, token);
            return Json(!check.Data.Any(s => s.Id != id));
        }
    }
}
