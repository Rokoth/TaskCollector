using System;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        protected override string defaultSort => "HId desc";

        protected override Func<Db.Interface.IRepository<Db.Model.UserHistory>, Db.Model.Filter<Db.Model.UserHistory>,
            CancellationToken, Task<Contract.Model.PagedResult<Db.Model.UserHistory>>> GetListFunc => (repo, filter, token) => repo.GetDeletedAsync(filter, token);

        protected override Expression<Func<Db.Model.UserHistory, bool>> GetFilter(UserHistoryFilter filter)
        {
            return s => (filter.Name == null || s.Name.Contains(filter.Name))
                && (filter.Id == null || s.Id == filter.Id);
        }
    }

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

        protected override Expression<Func<Db.Model.ClientHistory, bool>> GetFilter(ClientHistoryFilter filter)
        {
            return s => (filter.Name == null || s.Name.Contains(filter.Name))
                && (filter.Id == null || s.Id == filter.Id);
        }
    }

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

        protected override Expression<Func<Db.Model.MessageHistory, bool>> GetFilter(MessageHistoryFilter filter)
        {
            return s => (filter.Title == null || s.Title.Contains(filter.Title))
                && (filter.Id == null || s.Id == filter.Id)
                && (filter.ClientId == null || s.ClientId == filter.ClientId)
                 && (filter.From == null || s.ChangeDate >= filter.From)
                 && (filter.To == null || s.ChangeDate <= filter.To);
        }
    }

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

        protected override Expression<Func<Db.Model.MessageStatusHistory, bool>> GetFilter(MessageStatusHistoryFilter filter)
        {
            return s => (filter.Id == null || s.Id == filter.Id)
               && (filter.MessageId == null || s.MessageId == filter.MessageId)
                && (filter.From == null || s.ChangeDate >= filter.From)
                && (filter.To == null || s.ChangeDate <= filter.To);
        }
    }
}
