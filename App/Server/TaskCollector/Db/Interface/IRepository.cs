using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Db.Model;

namespace TaskCollector.Db.Interface
{
    public interface IRepository<T> where T : Entity
    {
        Task<IEnumerable<T>> GetAsync(Filter<T> filter, CancellationToken token);
        Task<T> GetAsync(Guid id, CancellationToken token);
    }
}
