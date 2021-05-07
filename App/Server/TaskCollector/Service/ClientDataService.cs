using System;
using System.Linq.Expressions;
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
        //protected override Func<Db.Model.Client, Contract.Model.ClientFilter, bool> GetFilter =>
        //     (s, filter) => (string.IsNullOrEmpty(filter.Name) || s.Name.ToLower().Contains(filter.Name.ToLower()))
        //                && (string.IsNullOrEmpty(filter.Login) || s.Login.ToLower().Contains(filter.Login.ToLower()))
        //                && (filter.UserId == s.UserId);

        protected override Func<Contract.Model.Client, Contract.Model.Client> EnrichFunc => null;

        public ClientDataService(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        protected override Expression<Func<Db.Model.Client, bool>> GetFilter(ClientFilter filter)
        {
            throw new NotImplementedException();
        }

        protected override string defaultSort => "Name";
    }
}
