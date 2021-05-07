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
        //protected override Func<Db.Model.Message, Contract.Model.MessageFilter, bool> GetFilter =>
        //     (s, t) => s.Title.ToLower().Contains(t.Title.ToLower());

        protected override Func<Contract.Model.Message, Contract.Model.Message> EnrichFunc => null;

        public MessageDataService(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        protected override Expression<Func<Db.Model.Message, bool>> GetFilter(MessageFilter filter)
        {
            throw new NotImplementedException();
        }

        protected override string defaultSort => "Name";
    }
}
