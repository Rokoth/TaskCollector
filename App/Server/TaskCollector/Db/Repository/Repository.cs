﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Db.Context;
using TaskCollector.Db.Interface;
using TaskCollector.Db.Model;

namespace TaskCollector.Db.Repository
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private IServiceProvider _serviceProvider;
        private ILogger _logger;

        public Repository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = _serviceProvider.GetRequiredService<ILogger<Repository<T>>>();
        }

        public async Task<IEnumerable<T>> GetAsync(Filter<T> filter, CancellationToken token)
        {
            try
            {
                var context = _serviceProvider.GetRequiredService<DbPgContext>();
                var all = await context.Set<T>().Where(filter.Selector)
                    .Skip(filter.Size * filter.Page).Take(filter.Size).ToListAsync();
                return all;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка в методе GetAsync Repository: {ex.Message} {ex.StackTrace}");
                throw new RepositoryException($"Ошибка в методе GetAsync Repository: {ex.Message}");
            }
        }
    }
}
