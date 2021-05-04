using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Contract.Model;

namespace TaskCollector.Service
{
    public interface IAddDataService<Tdto, TCreator> where Tdto : Entity
    {
        Task<Tdto> AddAsync(TCreator entity, CancellationToken token);
    }
}