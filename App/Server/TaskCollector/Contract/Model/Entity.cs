using System;
using System.Collections.Generic;

namespace TaskCollector.Contract.Model
{
    public class Entity
    {
        public Guid Id { get; }
    }

    public class MessageFilter : Filter<Message>
    {
        public MessageFilter(int size, int page, string sort, string title) : base(size, page, sort)
        {
            Title = title;
        }
        public string Title { get; }
    }

    public class MessageStatusFilter : Filter<MessageStatus>
    {
        public MessageStatusFilter(Guid messageId, int size, int page, string sort, string name) : base(size, page, sort)
        {
            Name = name;
            MessageId = messageId;
        }
        public string Name { get; }
        public Guid MessageId { get; }
    }

    public class UserFilter : Filter<User>
    {
        public UserFilter(int size, int page, string sort, string name) : base(size, page, sort)
        {
            Name = name;
        }
        public string Name { get; }
    }

    public class ClientFilter : Filter<Client>
    {
        public ClientFilter(int size, int page, string sort, string name) : base(size, page, sort)
        {
            Name = name;
        }
        public string Name { get; }
    }

    public abstract class Filter<T> where T : Entity
    {
        public Filter(int size, int page, string sort) 
        {
            Size = size;
            Page = page;
            Sort = sort;
        }
        public int Size { get; }
        public int Page { get; }
        public string Sort { get; }
    }

    public class PagedResult<T>
    {
        public PagedResult(IEnumerable<T> data, int allCount)
        {
            Data = data;
            AllCount = allCount;
        }
        public IEnumerable<T> Data { get; }
        public int AllCount { get; }
    }
}