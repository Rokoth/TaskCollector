using System;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Contract.Model;

namespace TaskCollector.Service
{
    public interface IDeleteDataService<Tdto> where Tdto : Entity
    {
        Task<Tdto> DeleteAsync(Guid id, CancellationToken token);
    }
}