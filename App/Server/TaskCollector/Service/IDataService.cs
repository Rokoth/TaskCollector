using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Contract.Model;

namespace TaskCollector.Service
{
    public interface IDataService
    {
        Task<PagedResult<User>> GetUsersAsync(UserFilter filter, CancellationToken token);
        Task<PagedResult<Client>> GetClientsAsync(ClientFilter filter, CancellationToken token);        
        Task<PagedResult<Message>> GetMessagesAsync(MessageFilter messageFilter, CancellationToken token);

        Task<User> GetUserAsync(Guid id, CancellationToken token);
        Task<Client> GetClientAsync(Guid id, CancellationToken token);
        Task<Message> AddMessageAsync(MessageCreator message, CancellationToken token);
        Task<Message> GetMessageAsync(Guid id, CancellationToken token);
        Task<Message> UpdateMessageAsync(MessageUpdater message, CancellationToken token);
        Task<MessageStatus> GetMessageStatusesAsync(MessageStatusFilter messageStatusFilter, CancellationToken token);
        Task<User> AddUserAsync(UserCreator creator, CancellationToken token);
        Task<User> DeleteUserAsync(Guid id, CancellationToken token);
        Task<User> UpdateUserAsync(UserUpdater updater, CancellationToken token);
        Task<ClaimsIdentity> Auth(ClientIdentity login, CancellationToken token);
        Task<ClaimsIdentity> Auth(UserIdentity login, CancellationToken token);
        Task<Message> DeleteMessageAsync(Guid id, CancellationToken token);
        Task<MessageStatus> GetMessageStatusAsync(Guid id, CancellationToken token);
        Task<MessageStatus> UpdateMessageStatusAsync(MessageStatusUpdater message, CancellationToken token);
    }

   
}