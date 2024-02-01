using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Contract.Model;

namespace TaskCollector.Service
{
    public class MessageHistoryDataService : DataGetService<Db.Model.MessageHistory,
        Contract.Model.MessageHistory,
        Contract.Model.MessageHistoryFilter>
    {
        public MessageHistoryDataService(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        protected override string defaultSort => "HId desc";

        protected override Func<Db.Interface.IRepository<Db.Model.MessageHistory>, Db.Model.Filter<Db.Model.MessageHistory>,
            CancellationToken, Task<Contract.Model.PagedResult<Db.Model.MessageHistory>>> GetListFunc => (repo, filter, token) => repo.GetDeletedAsync(filter, token);

        protected override Expression<Func<Db.Model.MessageHistory, bool>> GetFilter(MessageHistoryFilter filter, Guid userId)
        {
            return s => (filter.Title == null || s.Title.Contains(filter.Title))
                && (filter.Id == null || s.Id == filter.Id)
                && (filter.ClientId == null || s.ClientId == filter.ClientId)
                 && (filter.From == null || s.ChangeDate >= filter.From)
                 && (filter.To == null || s.ChangeDate <= filter.To);
        }
    }
}
