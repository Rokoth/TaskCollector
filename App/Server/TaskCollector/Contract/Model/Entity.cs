using System;
using System.Collections.Generic;

namespace TaskCollector.Contract.Model
{
    public class Entity
    {
        public Guid Id { get; set; }
    }

    public class MessageFilter : Filter<Message>
    {
        public MessageFilter(int size, int page, string sort, string name) : base(size, page, sort)
        {
            Name = name;
        }
        public string Name { get; set; }
    }

    public class UserFilter : Filter<User>
    {
        public UserFilter(int size, int page, string sort, string name) : base(size, page, sort)
        {
            Name = name;
        }
        public string Name { get; set; }
    }

    public class ClientFilter : Filter<Client>
    {
        public ClientFilter(int size, int page, string sort, string name) : base(size, page, sort)
        {
            Name = name;
        }
        public string Name { get; set; }
    }

    public abstract class Filter<T> where T : Entity
    {
        public Filter(int size, int page, string sort) 
        {
            Size = size;
            Page = page;
            Sort = sort;
        }
        public int Size { get; set; }
        public int Page { get; set; }
        public string Sort { get; set; }
    }

    public class PagedResult<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int AllCount { get; set; }
    }
}