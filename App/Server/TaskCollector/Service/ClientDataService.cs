using System;
using System.Linq.Expressions;


namespace TaskCollector.Service
{
    public class ClientDataService : DataService<
        Db.Model.Client, 
        Contract.Model.Client, 
        Contract.Model.ClientFilter, 
        Contract.Model.ClientCreator, 
        Contract.Model.ClientUpdater>
    {        
        protected override Func<Contract.Model.Client, Contract.Model.Client> EnrichFunc => null;

        public ClientDataService(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        protected override Expression<Func<Db.Model.Client, bool>> GetFilter(Contract.Model.ClientFilter filter)
        {
            return s => (string.IsNullOrEmpty(filter.Name) || s.Name.ToLower().Contains(filter.Name.ToLower()))
                && (string.IsNullOrEmpty(filter.Login) || s.Login.ToLower().Contains(filter.Login.ToLower()))
                && (filter.UserId == null || filter.UserId == s.UserId);
        }

        protected override string defaultSort => "Name";
    }
}
