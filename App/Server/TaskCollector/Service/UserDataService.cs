using System;

namespace TaskCollector.Service
{
    public class UserDataService : DataService<
        Db.Model.User, 
        Contract.Model.User, 
        Contract.Model.UserFilter, 
        Contract.Model.UserCreator, 
        Contract.Model.UserUpdater>
    {
        protected override Func<Db.Model.User, Contract.Model.UserFilter, bool> GetFilter =>
             (s, t) => s.Name.ToLower().Contains(t.Name.ToLower());

        protected override Func<Contract.Model.User, Contract.Model.User> EnrichFunc => null;

        public UserDataService(IServiceProvider serviceProvider) : base(serviceProvider)
        { 
        
        }
    }
}
