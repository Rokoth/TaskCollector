//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref 2
using System;
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

        protected virtual TEntity MapToEntityAdd(TCreator creator, Guid userId)
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
        public virtual async Task<Tdto> AddAsync(TCreator creator, Guid userId, CancellationToken token)
        {
            return await ExecuteAsync(async (repo) =>
            {
                if (await CheckAddRights(creator, userId))
                {
                    var entity = MapToEntityAdd(creator, userId);
                    var result = await repo.AddAsync(entity, true, token);
                    var prepare = _mapper.Map<Tdto>(result);
                    prepare = await Enrich(prepare, token);
                    return prepare;
                }
                throw new Exception("Ошибка доступа к записи: нет прав на добавление");
            });
        }               

        /// <summary>
        /// update item method
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Tdto> UpdateAsync(TUpdater entity, Guid userId, CancellationToken token)
        {
            return await ExecuteAsync(async (repo) =>
            {
                if (await CheckUpdateRights(entity, userId))
                {
                    var entry = await repo.GetAsync(entity.Id, token);
                    entry = UpdateFillFields(entity, entry);
                    TEntity result = await repo.UpdateAsync(entry, true, token);
                    var prepare = _mapper.Map<Tdto>(result);
                    prepare = await Enrich(prepare, token);
                    return prepare;
                }
                throw new Exception("Ошибка доступа к записи: нет прав на изменение");
            });
        }

        protected abstract TEntity UpdateFillFields(TUpdater entity, TEntity entry);

        /// <summary>
        /// delete item method
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Tdto> DeleteAsync(Guid id, Guid userId, CancellationToken token)
        {
            return await ExecuteAsync(async (repo) =>
            {
                var entry = await repo.GetAsync(id, token);
                if (await CheckDeleteRights(entry, userId))
                {
                    TEntity result = await repo.DeleteAsync(entry, true, token);
                    var prepare = _mapper.Map<Tdto>(result);
                    prepare = await Enrich(prepare, token);
                    return prepare;
                }
                throw new Exception("Ошибка доступа к записи: нет прав на удаление");
            });
        }

        protected abstract Task<bool> CheckDeleteRights(TEntity entry, Guid userId);

        protected abstract Task<bool> CheckUpdateRights(TUpdater entry, Guid userId);

        protected abstract Task<bool> CheckAddRights(TCreator entry, Guid userId);
    }
}
