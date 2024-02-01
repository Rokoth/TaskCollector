using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Contract.Model;

namespace TaskCollector.Service
{
    public class MessageDataService : DataService<
        Db.Model.Message,
        Contract.Model.Message,
        Contract.Model.MessageFilter,
        Contract.Model.MessageCreator,
        Contract.Model.MessageUpdater>
    {        

        public MessageDataService(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        protected override Expression<Func<Db.Model.Message, bool>> GetFilter(Contract.Model.MessageFilter filter, Guid userId)
        {

            return s => (string.IsNullOrEmpty(filter.Title) || s.Title.ToLower().Contains(filter.Title.ToLower())) &&
            (filter.ClientId == null || s.ClientId == filter.ClientId) &&
            (filter.Levels == null || filter.Levels.Count == 0 || filter.Levels.Contains(s.Level)) &&
            (filter.DateFrom == null || s.CreatedDate >= filter.DateFrom) &&
            (filter.DateTo == null || s.CreatedDate <= filter.DateTo);
        }

        public override async Task<Contract.Model.Message> AddAsync(Contract.Model.MessageCreator creator, Guid userId, CancellationToken token)
        {
            return await ExecuteAsync(async (repo) =>
            {
                var statusRepo = _serviceProvider.GetRequiredService<Db.Interface.IRepository<Db.Model.MessageStatus>>();
                var clientRepo = _serviceProvider.GetRequiredService<Db.Interface.IRepository<Db.Model.Client>>();
                var client = await clientRepo.GetAsync(creator.ClientId, token);
                var entity = MapToEntityAdd(creator, userId);
                var result = await repo.AddAsync(entity, false, token);
                var status = await statusRepo.AddAsync(new Db.Model.MessageStatus()
                { 
                  Description = "Добавление сообщения",
                  Id = Guid.NewGuid(),
                  IsDeleted = false,
                  IsLast = true,
                  MessageId = result.Id,                  
                  StatusDate = DateTimeOffset.Now,
                  StatusId = MessageStatusEnum.New,
                  UserId = client.UserId,
                  VersionDate = DateTimeOffset.Now
                }, false, token);
                await repo.SaveChangesAsync();
                var prepare = _mapper.Map<Contract.Model.Message>(result);
                prepare = await Enrich(prepare, token);
                return prepare;
            });
        }

        protected override Db.Model.Message UpdateFillFields(Contract.Model.MessageUpdater entity, Db.Model.Message entry)
        {
            entry.Description = entity.Description;
            entry.Level = entity.Level;
            entry.Title = entity.Title;
            entry.AddFields = entity.AddFields;
            entry.FeedbackContact = entity.FeedbackContact;
            return entry;
        }

        protected override async Task<bool> CheckDeleteRights(Db.Model.Message entry, Guid userId)
        {
            var clientRepo = _serviceProvider.GetRequiredService<Db.Interface.IRepository<Db.Model.Client>>();
            var client = await clientRepo.GetAsync(entry.ClientId, CancellationToken.None);
            return client.UserId == userId;
        }

        protected override async Task<bool> CheckUpdateRights(MessageUpdater entry, Guid userId)
        {
            var clientRepo = _serviceProvider.GetRequiredService<Db.Interface.IRepository<Db.Model.Client>>();
            var client = await clientRepo.GetAsync(entry.ClientId, CancellationToken.None);
            return client.UserId == userId;
        }

        protected override async Task<bool> CheckAddRights(MessageCreator entry, Guid userId)
        {
            var clientRepo = _serviceProvider.GetRequiredService<Db.Interface.IRepository<Db.Model.Client>>();
            var client = await clientRepo.GetAsync(entry.ClientId, CancellationToken.None);
            return client.UserId == userId;
        }

        protected override string defaultSort => "Title";
    }
}
