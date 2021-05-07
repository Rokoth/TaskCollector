using System;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using TaskCollector.Contract.Model;

namespace TaskCollector.Service
{
    public class UserDataService : DataService<
        Db.Model.User, 
        Contract.Model.User, 
        Contract.Model.UserFilter, 
        Contract.Model.UserCreator, 
        Contract.Model.UserUpdater>
    {
        protected override Expression<Func<Db.Model.User, bool>> GetFilter(Contract.Model.UserFilter filter)
        {
            return s => filter.Name == null || s.Name.Contains(filter.Name);
        }

        protected override Db.Model.User MapToEntityAdd(Contract.Model.UserCreator creator)
        {
            var entity = base.MapToEntityAdd(creator);
            entity.Password = SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(creator.Password));
            return entity;
        }

        protected override Func<Contract.Model.User, Contract.Model.User> EnrichFunc => null;

        protected override string defaultSort => "Name";

        public UserDataService(IServiceProvider serviceProvider) : base(serviceProvider)
        { 
        
        }
    }

    public class UserHistoryDataService : DataGetService<Db.Model.UserHistory,
        Contract.Model.UserHistory,
        Contract.Model.UserHistoryFilter>
    {
        public UserHistoryDataService(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        protected override string defaultSort => "Name";

        protected override Func<Contract.Model.UserHistory, Contract.Model.UserHistory> EnrichFunc => throw new NotImplementedException();

        protected override Expression<Func<Db.Model.UserHistory, bool>> GetFilter(UserHistoryFilter filter)
        {
            throw new NotImplementedException();
        }
    }
}
