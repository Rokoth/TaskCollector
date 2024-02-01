using System;

namespace TaskCollector.Contract.Model
{
    public class UserFilter : Filter<User>
    {
        public UserFilter(int size, int page, string sort, string name, string login) 
        {
            Name = name;
            Login = login;
            Page = page;
            Sort = sort;
            Size = size;
        }
        public string Name { get; }
        public string Login { get; }
    }

    public class UserHistoryFilter : Filter<UserHistory>
    {
        public UserHistoryFilter(int size, int page, string sort, string name, Guid? id)
        {
            Name = name;
            Id = id;
            Page = page;
            Sort = sort;
            Size = size;
        }
        public string Name { get; }
        public Guid? Id { get; }
    }

    public class ClientHistoryFilter : Filter<ClientHistory>
    {       
        public string Name { get; set; }
        public Guid? Id { get; set; }
    }

    public class MessageHistoryFilter : Filter<MessageHistory>
    {
        public MessageHistoryFilter(int size, int page, string sort, string title, Guid? id, Guid? clientId, DateTimeOffset? from, DateTimeOffset? to)
        {
            Title = title;
            Id = id;
            ClientId = clientId;
            From = from;
            To = to;
            Page = page;
            Sort = sort;
            Size = size;
        }
        public string Title { get; }
        public Guid? Id { get; }
        public Guid? ClientId { get; }
        public DateTimeOffset? From { get; }
        public DateTimeOffset? To { get; }
    }

    public class MessageStatusHistoryFilter : Filter<MessageStatusHistory>
    {
        public MessageStatusHistoryFilter(int size, int page, string sort, Guid? id, Guid? messageId, DateTimeOffset? from, DateTimeOffset? to)
        {            
            Id = id;
            MessageId = messageId;
            From = from;
            To = to;
            Page = page;
            Sort = sort;
            Size = size;
        }
        
        public Guid? Id { get; }
        public Guid? MessageId { get; }
        public DateTimeOffset? From { get; }
        public DateTimeOffset? To { get; }
    }
}