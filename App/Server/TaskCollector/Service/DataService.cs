using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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
                PagedResult<Db.Model.User> result = await repo.GetAsync(new Db.Model.Filter<Db.Model.User> { 
                    Size = filter.Size,
                    Page = filter.Page,
                    Sort = filter.Sort,
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
                PagedResult<Db.Model.Message> result = await repo.GetAsync(new Db.Model.Filter<Db.Model.Message>
                {
                    Size = filter.Size,
                    Page = filter.Page,
                    Sort = filter.Sort,
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

        public Task<User> DeleteUserAsync(Guid id, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateUserAsync(UserUpdater updater, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<Message> DeleteMessageAsync(Guid id, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<Message> GetMessageStatusAsync(Guid id, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task<ClaimsIdentity> Auth(ClientIdentity login, CancellationToken token)
        {
            var repo = _serviceProvider.GetRequiredService<Db.Interface.IRepository<Db.Model.Client>>();
            var password = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(login.Password));
            var client = (await repo.GetAsync(new Db.Model.Filter<Db.Model.Client>() { 
              Page = 0,
              Size = 10,
              Selector = s=>s.Login == login.Login && s.Password == password
            }, token)).Data.FirstOrDefault();
            if (client != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, client.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "Client")
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }
    }
}
