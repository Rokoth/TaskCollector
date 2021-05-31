using System;
using System.Linq;
using System.Linq.Expressions;
using TaskCollector.Contract.Model;

namespace TaskCollector.Service
{
    public class MessageStatusDataService : DataService<
        Db.Model.MessageStatus,
        Contract.Model.MessageStatus,
        Contract.Model.MessageStatusFilter,
        Contract.Model.MessageStatusCreator,
        Contract.Model.MessageStatusUpdater>
    {       
        public MessageStatusDataService(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        protected override Expression<Func<Db.Model.MessageStatus, bool>> GetFilter(MessageStatusFilter filter)
        {
            return s => (filter.MessageId == null || s.MessageId == filter.MessageId) &&
               (filter.Statuses == null || filter.Statuses.Contains(s.StatusId));
        }

        protected override Db.Model.MessageStatus UpdateFillFields(MessageStatusUpdater entity, Db.Model.MessageStatus entry)
        {          
            return entry;
        }

        protected override string defaultSort => "Name";
    }
}
