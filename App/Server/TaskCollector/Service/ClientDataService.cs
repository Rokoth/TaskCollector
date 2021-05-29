using System;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using TaskCollector.Contract.Model;

namespace TaskCollector.Service
{
    public class ClientDataService : DataService<
        Db.Model.Client, 
        Contract.Model.Client, 
        Contract.Model.ClientFilter, 
        Contract.Model.ClientCreator, 
        Contract.Model.ClientUpdater>
    {                

        public ClientDataService(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        protected override Expression<Func<Db.Model.Client, bool>> GetFilter(Contract.Model.ClientFilter filter)
        {
            return s => (string.IsNullOrEmpty(filter.Name) || s.Name.ToLower().Contains(filter.Name.ToLower()))
                && (string.IsNullOrEmpty(filter.Login) || s.Login.ToLower().Contains(filter.Login.ToLower()))
                && (filter.UserId == null || filter.UserId == s.UserId);
        }

        protected override Db.Model.Client MapToEntityAdd(Contract.Model.ClientCreator creator)
        {
            var entity = base.MapToEntityAdd(creator);
            entity.Password = SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(creator.Password));
            return entity;
        }

        protected override Db.Model.Client UpdateFillFields(ClientUpdater entity, Db.Model.Client entry)
        {
            entry.Description = entity.Description;
            entry.Login = entity.Login;
            entry.Name = entity.Name;
            entry.UserId = entity.UserId;
            if (entity.PasswordChanged)
            {
                entry.Password = SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(entity.Password));
            }
            return entry;
        }

        protected override string defaultSort => "Name";
    }
}
