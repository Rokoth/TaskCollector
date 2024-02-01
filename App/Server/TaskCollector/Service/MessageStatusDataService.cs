using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;
using TaskCollector.Contract.Model;

namespace TaskCollector.Service
{
    public class MessageStatusDataService : DataService<
        Db.Model.MessageStatus,
        Contract.Model.MessageStatus,
        Contract.Model.MessageStatusFilter,
        Contract.Model.MessageStatusCreator,
        Contract.Model.MessageStatusUpdater>
    {       
        public MessageStatusDataService(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        protected override Expression<Func<Db.Model.MessageStatus, bool>> GetFilter(MessageStatusFilter filter, Guid userId)
        {
            return s => (filter.MessageId == null || s.MessageId == filter.MessageId) &&
               (filter.Statuses == null || filter.Statuses.Contains(s.StatusId));
        }

        protected override Db.Model.MessageStatus UpdateFillFields(MessageStatusUpdater entity, Db.Model.MessageStatus entry)
        {
            entry.Description = entity.Description;
            entry.StatusDate = entity.StatusDate;
            entry.NextNotifyDate = entity.NextNotifyDate;            
            return entry;
        }

        protected override async Task<bool> CheckDeleteRights(Db.Model.MessageStatus entry, Guid userId)
        {
            var messageRepo = _serviceProvider.GetRequiredService<Db.Interface.IRepository<Db.Model.Message>>();
            var message = await messageRepo.GetAsync(entry.MessageId, CancellationToken.None);
            var clientRepo = _serviceProvider.GetRequiredService<Db.Interface.IRepository<Db.Model.Client>>();
            var client = await clientRepo.GetAsync(message.ClientId, CancellationToken.None);
            return client.UserId == userId;
        }

        protected override async Task<bool> CheckUpdateRights(MessageStatusUpdater entry, Guid userId)
        {
            var messageRepo = _serviceProvider.GetRequiredService<Db.Interface.IRepository<Db.Model.Message>>();
            var message = await messageRepo.GetAsync(entry.MessageId, CancellationToken.None);
            var clientRepo = _serviceProvider.GetRequiredService<Db.Interface.IRepository<Db.Model.Client>>();
            var client = await clientRepo.GetAsync(message.ClientId, CancellationToken.None);
            return client.UserId == userId;
        }

        protected override async Task<bool> CheckAddRights(MessageStatusCreator entry, Guid userId)
        {
            var messageRepo = _serviceProvider.GetRequiredService<Db.Interface.IRepository<Db.Model.Message>>();
            var message = await messageRepo.GetAsync(entry.MessageId, CancellationToken.None);
            var clientRepo = _serviceProvider.GetRequiredService<Db.Interface.IRepository<Db.Model.Client>>();
            var client = await clientRepo.GetAsync(message.ClientId, CancellationToken.None);
            return client.UserId == userId;
        }

        protected override string defaultSort => "StatusDate";
    }
}
