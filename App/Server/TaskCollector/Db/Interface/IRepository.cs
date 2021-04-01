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
        Task<IEnumerable<User>> GetAsync(UserFilter userFilter, CancellationToken token);
    }
}
