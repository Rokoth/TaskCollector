using System;

namespace TaskCollector.Contract.Model
{
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
}