﻿using System;
using System.Collections.Generic;
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
    }

   
}