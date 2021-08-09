using System;

namespace TaskCollector.Contract.Model
{
    public class UserFilter : Filter<User>
    {
        public UserFilter(int size, int page, string sort, string name, string login) : base(size, page, sort)
        {
            Name = name;
            Login = login;
        }
        public string Name { get; }
        public string Login { get; }
    }

    public class UserHistoryFilter : Filter<UserHistory>
    {
        public UserHistoryFilter(int size, int page, string sort, string name, Guid? id) : base(size, page, sort)
        {
            Name = name;
            Id = id;
        }
        public string Name { get; }
        public Guid? Id { get; }
    }

    public class ClientHistoryFilter : Filter<ClientHistory>
    {
        public ClientHistoryFilter(int size, int page, string sort, string name, Guid? id) : base(size, page, sort)
        {
            Name = name;
            Id = id;
        }
        public string Name { get; }
        public Guid? Id { get; }
    }

    public class MessageHistoryFilter : Filter<MessageHistory>
    {
        public MessageHistoryFilter(int size, int page, string sort, string title, Guid? id, Guid? clientId, DateTimeOffset? from, DateTimeOffset? to) : base(size, page, sort)
        {
            Title = title;
            Id = id;
            ClientId = clientId;
            From = from;
            To = to;
        }
        public string Title { get; }
        public Guid? Id { get; }
        public Guid? ClientId { get; }
        public DateTimeOffset? From { get; }
        public DateTimeOffset? To { get; }
    }
}