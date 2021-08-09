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
            return s => (filter.Name == null || s.Name.Contains(filter.Name)) && (filter.Login == null || s.Login.Contains(filter.Login));
        }

        protected override Db.Model.User MapToEntityAdd(Contract.Model.UserCreator creator)
        {
            var entity = base.MapToEntityAdd(creator);
            entity.Password = SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(creator.Password));
            return entity;
        }

        protected override Db.Model.User UpdateFillFields(UserUpdater entity, Db.Model.User entry)
        {
            entry.Description = entity.Description;
            entry.Login = entity.Login;
            entry.Name = entity.Name;
            if (entity.PasswordChanged)
            {
                entry.Password = SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(entity.Password));
            }
            return entry;
        }

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
               
        protected override Expression<Func<Db.Model.UserHistory, bool>> GetFilter(UserHistoryFilter filter)
        {
            throw new NotImplementedException();
        }
    }
}
