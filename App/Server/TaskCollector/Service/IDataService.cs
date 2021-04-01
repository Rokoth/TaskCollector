using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TaskCollector.Service
{
    public interface IDataService
    {

        Task<IEnumerable<Contract.Model.User>> GetUsers(Contract.Model.UserFilter filter, CancellationToken token);
    }
}