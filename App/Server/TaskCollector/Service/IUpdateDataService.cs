using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Contract.Model;

namespace TaskCollector.Service
{
    public interface IUpdateDataService<Tdto, TUpdater> where Tdto : Entity
    {
        Task<Tdto> UpdateAsync(TUpdater entity, CancellationToken token);
    }
}