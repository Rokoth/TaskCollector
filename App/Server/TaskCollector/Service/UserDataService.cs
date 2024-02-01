using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Contract.Model;
using TaskCollector.Db.Model;

namespace TaskCollector.Service
{
    public class UserDataService : DataService<
        Db.Model.User, 
        Contract.Model.User, 
        Contract.Model.UserFilter, 
        Contract.Model.UserCreator, 
        Contract.Model.UserUpdater>
    {
        protected override Expression<Func<Db.Model.User, bool>> GetFilter(Contract.Model.UserFilter filter, Guid userId)
        {
            return s => (filter.Name == null || s.Name.Contains(filter.Name)) && (filter.Login == null || s.Login.Contains(filter.Login));
        }

        protected override Db.Model.User MapToEntityAdd(Contract.Model.UserCreator creator, Guid userId)
        {
            var entity = base.MapToEntityAdd(creator, userId);
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

        protected override async Task<bool> CheckDeleteRights(Db.Model.User entry, Guid userId)
        {
            var userRepo = _serviceProvider.GetRequiredService<Db.Interface.IRepository<Db.Model.User>>();
            var user = await userRepo.GetAsync(userId, CancellationToken.None);
            return user.Login.ToLower() == "admin";
        }

        protected override async Task<bool> CheckUpdateRights(UserUpdater entry, Guid userId)
        {
            var userRepo = _serviceProvider.GetRequiredService<Db.Interface.IRepository<Db.Model.User>>();
            var user = await userRepo.GetAsync(userId, CancellationToken.None);
            return user.Login.ToLower() == "admin";
        }

        protected override async Task<bool> CheckAddRights(UserCreator entry, Guid userId)
        {
            var userRepo = _serviceProvider.GetRequiredService<Db.Interface.IRepository<Db.Model.User>>();
            var user = await userRepo.GetAsync(userId, CancellationToken.None);
            return user.Login.ToLower() == "admin";
        }
    }
}
