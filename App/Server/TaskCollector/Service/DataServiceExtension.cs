using Microsoft.Extensions.DependencyInjection;

namespace TaskCollector.Service
{
    public static class DataServiceExtension
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            services.AddDataService<UserDataService, Db.Model.User, Contract.Model.User, 
                Contract.Model.UserFilter, Contract.Model.UserCreator, Contract.Model.UserUpdater> ();
            services.AddDataService<ClientDataService, Db.Model.Client, Contract.Model.Client,
                Contract.Model.ClientFilter, Contract.Model.ClientCreator, Contract.Model.ClientUpdater>();
            services.AddDataService<MessageDataService, Db.Model.Message, Contract.Model.Message,
                Contract.Model.MessageFilter, Contract.Model.MessageCreator, Contract.Model.MessageUpdater>();
            services.AddDataService<MessageStatusDataService, Db.Model.MessageStatus, Contract.Model.MessageStatus,
                Contract.Model.MessageStatusFilter, Contract.Model.MessageStatusCreator, Contract.Model.MessageStatusUpdater>();

            services.AddScoped<IGetDataService<Contract.Model.UserHistory, Contract.Model.UserHistoryFilter>, UserHistoryDataService>();
            services.AddScoped<IGetDataService<Contract.Model.ClientHistory, Contract.Model.ClientHistoryFilter>, ClientHistoryDataService>();
            services.AddScoped<IGetDataService<Contract.Model.MessageHistory, Contract.Model.MessageHistoryFilter>, MessageHistoryDataService>();
            services.AddScoped<IGetDataService<Contract.Model.MessageStatusHistory, Contract.Model.MessageStatusHistoryFilter>, MessageStatusHistoryDataService>();
            
            services.AddScoped<IAuthService, AuthService>();



            return services;
        }

        private static IServiceCollection AddDataService<TService, TEntity, Tdto, TFilter, TCreator, TUpdater>(this IServiceCollection services)
            where TEntity : Db.Model.Entity
            where TService : DataService<TEntity, Tdto, TFilter, TCreator, TUpdater>
            where Tdto : Contract.Model.Entity
            where TFilter : Contract.Model.Filter<Tdto>
            where TUpdater: Contract.Model.IEntity
        {
            services.AddScoped<IGetDataService<Tdto, TFilter>, TService>();
            services.AddScoped<IAddDataService<Tdto, TCreator>, TService>();
            services.AddScoped<IUpdateDataService<Tdto, TUpdater>, TService>();
            services.AddScoped<IDeleteDataService<Tdto>, TService>();
            return services;
        }
    }
}
