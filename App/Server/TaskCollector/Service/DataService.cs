//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0

//ref 1
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    public abstract class DataGetService<TEntity, Tdto, TFilter> :
        IGetDataService<Tdto, TFilter>        
        where TEntity : Db.Model.IEntity
        where Tdto : Contract.Model.Entity
        where TFilter : Contract.Model.Filter<Tdto>
    {
        protected IServiceProvider _serviceProvider;
        protected IMapper _mapper;

        protected abstract string defaultSort { get; }

        /// <summary>
        /// function for modify client filter to db filter
        /// </summary>
        protected abstract Expression<Func<TEntity, bool>> GetFilter(TFilter filter);


        /// <summary>
        /// function for enrichment data item
        /// </summary>
        protected virtual async Task<Tdto> Enrich(Tdto entity, CancellationToken token)
        {
            return entity;
        }

        /// <summary>
        /// function for enrichment data item
        /// </summary>
        protected virtual async Task<IEnumerable<Tdto>> Enrich(IEnumerable<Tdto> entities, CancellationToken token)
        {
            return entities;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider"></param>
        public DataGetService(IServiceProvider serviceProvider)
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
                //Expression<Func<TEntity, bool>> expr = s => GetFilter(s, filter);
                //var func = expr.Compile(true);

                string sort = filter.Sort;
                if (string.IsNullOrEmpty(sort))
                {
                    sort = defaultSort;
                }
                var result = await repo.GetAsync(new Db.Model.Filter<TEntity>
                {
                    Size = filter.Size,
                    Page = filter.Page,
                    Sort = sort,
                    Selector = GetFilter(filter)
                }, token);
                var prepare = result.Data.Select(s => _mapper.Map<Tdto>(s));
                prepare = await Enrich(prepare, token);
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
                prepare = await Enrich(prepare, token);
                return prepare;
            });
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

    /// <summary>
    /// Data get, manipulation and prepare service
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="Tdto"></typeparam>
    /// <typeparam name="TFilter"></typeparam>
    /// <typeparam name="TCreator"></typeparam>
    /// <typeparam name="TUpdater"></typeparam>
    public abstract class DataService<TEntity, Tdto, TFilter, TCreator, TUpdater> :
        DataGetService<TEntity, Tdto, TFilter>,
        IGetDataService<Tdto, TFilter>,
        IAddDataService<Tdto, TCreator>,
        IUpdateDataService<Tdto, TUpdater>, 
        IDeleteDataService<Tdto> 
        where TEntity : Db.Model.Entity
        where Tdto : Contract.Model.Entity
        where TFilter : Contract.Model.Filter<Tdto>
        where TUpdater : Contract.Model.IEntity
    {
       
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider"></param>
        public DataService(IServiceProvider serviceProvider): base(serviceProvider)
        {
          
        }

        protected virtual TEntity MapToEntityAdd(TCreator creator)
        {
            var result = _mapper.Map<TEntity>(creator);
            result.Id = Guid.NewGuid();
            result.IsDeleted = false;
            result.VersionDate = DateTimeOffset.Now;
            return result;
        }

        /// <summary>
        /// add item method
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Tdto> AddAsync(TCreator creator, CancellationToken token)
        {
            return await ExecuteAsync(async (repo) =>
            {
                var entity = MapToEntityAdd(creator);
                var result = await repo.AddAsync(entity, true, token);
                var prepare = _mapper.Map<Tdto>(result);
                prepare = await Enrich(prepare, token);
                return prepare;
            });
        }

        /// <summary>
        /// update item method
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Tdto> UpdateAsync(TUpdater entity, CancellationToken token)
        {
            return await ExecuteAsync(async (repo) =>
            {
                var entry = await repo.GetAsync(entity.Id, token);
                entry = UpdateFillFields(entity, entry);
                TEntity result = await repo.UpdateAsync(entry, true, token);
                var prepare = _mapper.Map<Tdto>(result);
                prepare = await Enrich(prepare, token);
                return prepare;
            });
        }

        protected abstract TEntity UpdateFillFields(TUpdater entity, TEntity entry);

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
    }
}
