using System;

namespace TaskCollector.Contract.Model
{
    public class ClientFilter : Filter<Client>
    {
        public ClientFilter(int size, int page, string sort, string name, string login, Guid userId) : base(size, page, sort)
        {
            Name = name;
            Login = login;
            UserId = userId;
        }
        public string Name { get; }
        public string Login { get; }
        public Guid UserId { get; }
    }
}