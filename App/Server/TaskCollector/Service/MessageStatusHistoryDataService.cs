using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Contract.Model;

namespace TaskCollector.Service
{
    public class MessageStatusHistoryDataService : DataGetService<Db.Model.MessageStatusHistory,
        Contract.Model.MessageStatusHistory,
        Contract.Model.MessageStatusHistoryFilter>
    {
        public MessageStatusHistoryDataService(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        protected override string defaultSort => "HId desc";

        protected override Func<Db.Interface.IRepository<Db.Model.MessageStatusHistory>, Db.Model.Filter<Db.Model.MessageStatusHistory>,
            CancellationToken, Task<Contract.Model.PagedResult<Db.Model.MessageStatusHistory>>> GetListFunc => (repo, filter, token) => repo.GetDeletedAsync(filter, token);

        protected override Expression<Func<Db.Model.MessageStatusHistory, bool>> GetFilter(MessageStatusHistoryFilter filter, Guid userId)
        {
            return s => (filter.Id == null || s.Id == filter.Id)
               && (filter.MessageId == null || s.MessageId == filter.MessageId)
                && (filter.From == null || s.ChangeDate >= filter.From)
                && (filter.To == null || s.ChangeDate <= filter.To);
        }
    }
}
