//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0

//ref 1
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TaskCollector.Service
{
    /// <summary>
    /// Data get, manipulation and prepare service
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="Tdto"></typeparam>
    /// <typeparam name="TFilter"></typeparam>
    /// <typeparam name="TCreator"></typeparam>
    /// <typeparam name="TUpdater"></typeparam>
    public abstract class DataService<TEntity, Tdto, TFilter, TCreator, TUpdater> :
        IGetDataService<Tdto, TFilter>,
        IAddDataService<Tdto, TCreator>,
        IUpdateDataService<Tdto, TUpdater>, 
        IDeleteDataService<Tdto> 
        where TEntity : Db.Model.Entity
        where Tdto : Contract.Model.Entity
        where TFilter : Contract.Model.Filter<Tdto>
    {
        protected IServiceProvider _serviceProvider;
        protected IMapper _mapper;

        /// <summary>
        /// function for modify client filter to db filter
        /// </summary>
        protected abstract Func<TEntity, TFilter, bool> GetFilter { get; }

        /// <summary>
        /// function for enrichment data
        /// </summary>
        protected abstract Func<Tdto, Tdto> EnrichFunc { get; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider"></param>
        public DataService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _mapper = _serviceProvider.GetRequiredService<IMapper>();
        }

        /// <summary>
        /// Get items method
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Contract.Model.PagedResult<Tdto>> GetAsync(TFilter filter, CancellationToken token)
        {
            return await ExecuteAsync(async (repo) =>
            {                
                var result = await repo.GetAsync(new Db.Model.Filter<TEntity>
                {
                    Size = filter.Size,
                    Page = filter.Page,
                    Sort = filter.Sort,
                    Selector = s => GetFilter(s, filter)
                }, token);
                var prepare = result.Data.Select(s => _mapper.Map<Tdto>(s));
                if (EnrichFunc != null)
                {
                    prepare = prepare.Select(s => EnrichFunc(s));
                }
                return new Contract.Model.PagedResult<Tdto>(prepare, result.AllCount);
            });            
        }

        /// <summary>
        /// get item method
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Tdto> GetAsync(Guid id, CancellationToken token)
        {
            return await ExecuteAsync(async (repo) =>
            {                
                var result = await repo.GetAsync(id, token);
                var prepare = _mapper.Map<Tdto>(result);
                if (EnrichFunc != null)
                {
                    prepare = EnrichFunc(prepare);
                }
                return prepare;
            });           
        }

        /// <summary>
        /// add item method
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<Tdto> AddAsync(TCreator entity, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// update item method
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<Tdto> UpdateAsync(TUpdater entity, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// delete item method
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<Tdto> DeleteAsync(Guid id, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// execution wrapper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="execute"></param>
        /// <returns></returns>
        protected async Task<T> ExecuteAsync<T>(Func<Db.Interface.IRepository<TEntity>, Task<T>> execute)
        {            
            try
            {
                var repo = _serviceProvider.GetRequiredService<Db.Interface.IRepository<TEntity>>();
                return await execute(repo);
            }
            catch (DataServiceException)
            {
                throw;
            }
            catch (Db.Repository.RepositoryException ex)
            {
                throw new DataServiceException(ex.Message);
            }
        }        
    }
}
