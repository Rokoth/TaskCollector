using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Contract.Model;

namespace TaskCollector.Service
{
    public class ClientHistoryDataService : DataGetService<Db.Model.ClientHistory,
        Contract.Model.ClientHistory,
        Contract.Model.ClientHistoryFilter>
    {
        public ClientHistoryDataService(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        protected override string defaultSort => "HId desc";

        protected override Func<Db.Interface.IRepository<Db.Model.ClientHistory>, Db.Model.Filter<Db.Model.ClientHistory>,
            CancellationToken, Task<Contract.Model.PagedResult<Db.Model.ClientHistory>>> GetListFunc => (repo, filter, token) => repo.GetDeletedAsync(filter, token);

        protected override Expression<Func<Db.Model.ClientHistory, bool>> GetFilter(ClientHistoryFilter filter, Guid userId)
        {
            return s => (filter.Name == null || s.Name.Contains(filter.Name))
                && (filter.Id == null || s.Id == filter.Id);
        }
    }
}
