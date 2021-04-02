using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Contract.Model;

namespace TaskCollector.Service
{
    public interface IDataService
    {

        Task<IEnumerable<Contract.Model.User>> GetUsers(Contract.Model.UserFilter filter, CancellationToken token);
        Task<Client> GetClient(Guid id, CancellationToken token);
    }
}