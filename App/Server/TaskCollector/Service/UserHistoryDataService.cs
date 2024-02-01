using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Contract.Model;

namespace TaskCollector.Service
{
    public class UserHistoryDataService : DataGetService<Db.Model.UserHistory,
        Contract.Model.UserHistory,
        Contract.Model.UserHistoryFilter>
    {
        public UserHistoryDataService(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        protected override string defaultSort => "HId desc";

        protected override Func<Db.Interface.IRepository<Db.Model.UserHistory>, Db.Model.Filter<Db.Model.UserHistory>,
            CancellationToken, Task<Contract.Model.PagedResult<Db.Model.UserHistory>>> GetListFunc => (repo, filter, token) => repo.GetDeletedAsync(filter, token);

        protected override Expression<Func<Db.Model.UserHistory, bool>> GetFilter(UserHistoryFilter filter, Guid userId)
        {
            return s => (filter.Name == null || s.Name.Contains(filter.Name))
                && (filter.Id == null || s.Id == filter.Id);
        }
    }
}
