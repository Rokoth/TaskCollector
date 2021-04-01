using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Db.Interface;
using TaskCollector.Db.Model;

namespace TaskCollector.Db.Repository
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        public Task<IEnumerable<User>> GetAsync(UserFilter userFilter, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
