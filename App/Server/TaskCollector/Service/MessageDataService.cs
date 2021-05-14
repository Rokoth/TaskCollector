using System;
using System.Linq.Expressions;
using TaskCollector.Contract.Model;

namespace TaskCollector.Service
{
    public class MessageDataService : DataService<
        Db.Model.Message,
        Contract.Model.Message,
        Contract.Model.MessageFilter,
        Contract.Model.MessageCreator,
        Contract.Model.MessageUpdater>
    {        
        protected override Func<Contract.Model.Message, Contract.Model.Message> EnrichFunc => s => {
            return s;
         };

        public MessageDataService(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        protected override Expression<Func<Db.Model.Message, bool>> GetFilter(MessageFilter filter)
        {

            return s => (string.IsNullOrEmpty(filter.Title) || s.Title.ToLower().Contains(filter.Title.ToLower())) &&
            (filter.ClientId == null || s.ClientId == filter.ClientId) &&
            (filter.Levels == null || filter.Levels.Contains(s.Level)) &&
            (filter.DateFrom == null || s.CreatedDate >= filter.DateFrom) &&
            (filter.DateTo == null || s.CreatedDate <= filter.DateTo);
        }

        protected override string defaultSort => "Title";
    }
}
