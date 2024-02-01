﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskCollector.Service
{
    public interface IGetDataService<Tdto, TFilter>
        where Tdto : Contract.Model.Entity
        where TFilter : Contract.Model.Filter<Tdto>
    {
        Task<Tdto> GetAsync(Guid id, Guid userId, CancellationToken token);
        Task<Contract.Model.PagedResult<Tdto>> GetAsync(TFilter filter, Guid userId, CancellationToken token);
    }
}
