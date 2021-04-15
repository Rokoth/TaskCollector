using Microsoft.EntityFrameworkCore;
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

        public async Task<Contract.Model.PagedResult<T>> GetAsync(Filter<T> filter, CancellationToken token)
        {
            try
            {
                var context = _serviceProvider.GetRequiredService<DbPgContext>();
                var all = context.Set<T>().Where(filter.Selector);                
                var result = await all.Skip(filter.Size * filter.Page).Take(filter.Size).ToListAsync();
                return new Contract.Model.PagedResult<T>(result, await all.CountAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка в методе GetAsync Repository: {ex.Message} {ex.StackTrace}");
                throw new RepositoryException($"Ошибка в методе GetAsync Repository: {ex.Message}");
            }
        }

        public async Task<T> GetAsync(Guid id, CancellationToken token)
        {
            try
            {
                var context = _serviceProvider.GetRequiredService<DbPgContext>();
                var item = await context.Set<T>().Where(s=>s.Id == id).FirstOrDefaultAsync();                
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка в методе GetAsync Repository: {ex.Message} {ex.StackTrace}");
                throw new RepositoryException($"Ошибка в методе GetAsync Repository: {ex.Message}");
            }
        }
    }
}
