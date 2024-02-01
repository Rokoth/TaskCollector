//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref2
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Common;
using TaskCollector.Contract.Model;

namespace TaskCollector.Service
{
    /// <summary>
    /// realization dataservice for Client
    /// </summary>
    public class ClientDataService : DataService<
        Db.Model.Client, 
        Contract.Model.Client, 
        Contract.Model.ClientFilter, 
        Contract.Model.ClientCreator, 
        Contract.Model.ClientUpdater>
    {                

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ClientDataService(IServiceProvider serviceProvider) : base(serviceProvider) { }

        /// <summary>
        /// function for enrichment data item
        /// </summary>
        protected override async Task<Contract.Model.Client> Enrich(Contract.Model.Client entity, CancellationToken token)
        {
            entity.User = await GetUserName(entity, token);
            return entity;
        }

        private async Task<string> GetUserName(Contract.Model.Client entity, CancellationToken token)
        {
            var userRepo = _serviceProvider.GetRequiredService<Db.Interface.IRepository<Db.Model.User>>();
            return (await userRepo.GetAsync(entity.UserId, token))?.Name;
        }

        /// <summary>
        /// function for enrichment data item
        /// </summary>
        protected override async Task<IEnumerable<Contract.Model.Client>> Enrich(IEnumerable<Contract.Model.Client> entities, CancellationToken token)
        {
            var response = new List<Contract.Model.Client>();
            foreach (var entity in entities)
            {
                entity.User = await GetUserName(entity, token);
                response.Add(entity);
            }
            return response;
        }

        /// <summary>
        /// Create filter for query
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        protected override Expression<Func<Db.Model.Client, bool>> GetFilter(Contract.Model.ClientFilter filter, Guid userId)
        {
            return s => (string.IsNullOrEmpty(filter.Name) || s.Name.ToLower().Contains(filter.Name.ToLower()))
                && (string.IsNullOrEmpty(filter.Login) || s.Login.ToLower().Contains(filter.Login.ToLower()))
                && (userId == null || userId == s.UserId);
        }

        /// <summary>
        /// Map to db model from creator
        /// </summary>
        /// <param name="creator"></param>
        /// <returns></returns>
        protected override Db.Model.Client MapToEntityAdd(Contract.Model.ClientCreator creator, Guid userId)
        {
            var entity = base.MapToEntityAdd(creator, userId);
            entity.Password = HelperExtension.EncryptPassword(creator.Password);
            entity.UserId = userId;
            return entity;
        }

        /// <summary>
        /// update fields from updater
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        protected override Db.Model.Client UpdateFillFields(Contract.Model.ClientUpdater entity, Db.Model.Client entry)
        {
            entry.Description = entity.Description;
            entry.Login = entity.Login;
            entry.Name = entity.Name;
            entry.UserId = entity.UserId;
            entry.MapRules = entity.MapRules;
            if (entity.PasswordChanged)
            {
                entry.Password = HelperExtension.EncryptPassword(entity.Password);
            }
            return entry;
        }

        protected override async Task<bool> CheckDeleteRights(Db.Model.Client entry, Guid userId)
        {
            return entry.UserId == userId;
        }

        protected override async Task<bool> CheckUpdateRights(ClientUpdater entry, Guid userId)
        {
            return entry.UserId == userId;
        }

        protected override async Task<bool> CheckAddRights(ClientCreator entry, Guid userId)
        {
            return true;
        }

        /// <summary>
        /// default field for sort
        /// </summary>
        protected override string defaultSort => "Name";
    }
}
