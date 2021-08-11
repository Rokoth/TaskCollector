//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref2

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Db.Context;
using TaskCollector.Db.Interface;
using TaskCollector.Db.Model;
using System.Linq.Dynamic.Core;

namespace TaskCollector.Db.Repository
{
    /// <summary>
    /// Репозиторий
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public Repository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = _serviceProvider.GetRequiredService<ILogger<Repository<T>>>();
        }

        /// <summary>
        /// Метод добавления модели в базу
        /// </summary>
        /// <param name="entity">модель</param>
        /// <param name="withSave">с сохраннеием</param>
        /// <param name="token">токен</param>
        /// <returns>модель</returns>
        public async Task<T> AddAsync(T entity, bool withSave, CancellationToken token)
        {
            return await ExecuteAsync(async (context) => {
                entity.VersionDate = DateTimeOffset.Now;
                var item = context.Set<T>().Add(entity).Entity;
                if (withSave) await context.SaveChangesAsync();
                return item;
            }, "GetAsync");            
        }

        /// <summary>
        /// Метод получения отфильтрованного списка моделей с постраничной отдачей
        /// </summary>
        /// <param name="filter">фильтр</param>
        /// <param name="token">токен</param>
        /// <returns>список моделей</returns>
        public async Task<Contract.Model.PagedResult<T>> GetAsync(Filter<T> filter, CancellationToken token)
        {
            return await ExecuteAsync(async (context) => {
                var all = context.Set<T>().Where(filter.Selector).Where(s=>!s.IsDeleted);
                if (!string.IsNullOrEmpty(filter.Sort))
                {
                    all = all.OrderBy(filter.Sort);
                }
                var result = await all
                    .Skip(filter.Size * filter.Page)
                    .Take(filter.Size)
                    .ToListAsync();
                return new Contract.Model.PagedResult<T>(result, await all.CountAsync());
            }, "GetAsync");            
        }

        public async Task<Contract.Model.PagedResult<T>> GetDeletedAsync(Filter<T> filter, CancellationToken token)
        {
            return await ExecuteAsync(async (context) => {
                var all = context.Set<T>().Where(filter.Selector);
                if (!string.IsNullOrEmpty(filter.Sort))
                {
                    all = all.OrderBy(filter.Sort);
                }
                var result = await all
                    .Skip(filter.Size * filter.Page)
                    .Take(filter.Size)
                    .ToListAsync();
                return new Contract.Model.PagedResult<T>(result, await all.CountAsync());
            }, "GetAsync");
        }

        /// <summary>
        /// Метод получения модели по id
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        public async Task<T> GetAsync(Guid id, CancellationToken token)
        {
            return await ExecuteAsync(async (context) => {               
                return await context.Set<T>()
                    .Where(s => s.Id == id && !s.IsDeleted).FirstOrDefaultAsync();                
            }, "GetAsync");            
        }

        public async Task<T> UpdateAsync(T entity, bool withSave, CancellationToken token)
        {
            return await ExecuteAsync(async (context) => {
                entity.VersionDate = DateTimeOffset.Now;
                var item = context.Set<T>().Update(entity).Entity;
                if (withSave) await context.SaveChangesAsync();
                return item;
            }, "UpdateAsync");
        }

        public async Task<T> DeleteAsync(T entity, bool withSave, CancellationToken token)
        {
            return await ExecuteAsync(async (context) => {
                entity.IsDeleted = true;
                entity.VersionDate = DateTimeOffset.Now;
                var item = context.Set<T>().Update(entity).Entity;
                if (withSave) await context.SaveChangesAsync();
                return item;
            }, "DeleteAsync");
        }

        private async Task<TEx> ExecuteAsync<TEx>(Func<DbPgContext, Task<TEx>> action, string method)
        {
            try
            {
                var context = _serviceProvider.GetRequiredService<DbPgContext>();
                return await action(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка в методе {method} Repository: {ex.Message} {ex.StackTrace}");
                throw new RepositoryException($"Ошибка в методе {method} Repository: {ex.Message}");
            }
        }

        public void ClearChangeTracker()
        {
            var context = _serviceProvider.GetRequiredService<DbPgContext>();
            context.ChangeTracker.Clear();
        }

        public async Task SaveChangesAsync()
        {
            var context = _serviceProvider.GetRequiredService<DbPgContext>();
            await context.SaveChangesAsync();
        }
    }
}
