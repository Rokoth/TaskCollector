using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Contract.Model;

namespace TaskCollector.Service
{
    public class DataService: IDataService
    {
        private IServiceProvider _serviceProvider;
        private IMapper _mapper;
        public DataService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _mapper = _serviceProvider.GetRequiredService<IMapper>();
        }

        public async Task<PagedResult<Contract.Model.User>> GetUsersAsync(Contract.Model.UserFilter filter, CancellationToken token)
        {
            try
            {
                var repo = _serviceProvider.GetRequiredService<Db.Interface.IRepository<Db.Model.User>>();
                PagedResult<Db.Model.User> result = await repo.GetAsync(new Db.Model.UserFilter { 
                   Size = filter.Size,
                   Page = filter.Page,
                   Selector = s=>s.Name.ToLower().Contains(filter.Name.ToLower())
                }, token);
                return new PagedResult<User>(result.Data.Select(s => _mapper.Map<Contract.Model.User>(s)), result.AllCount);
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

        public async Task<Contract.Model.User> GetUserAsync(Guid id, CancellationToken token)
        {
            try
            {
                var repo = _serviceProvider.GetRequiredService<Db.Interface.IRepository<Db.Model.User>>();
                var result = await repo.GetAsync(id, token);
                return _mapper.Map<Contract.Model.User>(result);
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

        public async Task<PagedResult<Message>> GetMessagesAsync(MessageFilter filter, CancellationToken token)
        {
            try
            {
                var repo = _serviceProvider.GetRequiredService<Db.Interface.IRepository<Db.Model.Message>>();
                PagedResult<Db.Model.Message> result = await repo.GetAsync(new Db.Model.MessageFilter
                {
                    Size = filter.Size,
                    Page = filter.Page,
                    Selector = s => s.Title.ToLower().Contains(filter.Title.ToLower())
                }, token);
                return new PagedResult<Message>(result.Data.Select(s => _mapper.Map<Contract.Model.Message>(s)), result.AllCount);
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

        public Task<PagedResult<Client>> GetClientsAsync(ClientFilter filter, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<Client> GetClientAsync(Guid id, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<Message> AddMessageAsync(MessageCreator message, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<Message> GetMessageAsync(Guid id, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<Message> UpdateMessageAsync(MessageUpdater message, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<MessageStatus> GetMessageStatusesAsync(MessageStatusFilter messageStatusFilter, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<User> AddUserAsync(UserCreator creator, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
