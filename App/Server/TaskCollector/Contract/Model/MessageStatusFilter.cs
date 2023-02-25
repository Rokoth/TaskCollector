using System;

namespace TaskCollector.Contract.Model
{
    public class MessageStatusFilter : Filter<MessageStatus>
    {
        public MessageStatusFilter(Guid? messageId, int size, int page, string sort, MessageStatusEnum[] statuses) : base(size, page, sort)
        {            
            MessageId = messageId;
            Statuses = statuses;
        }
       
        public Guid? MessageId { get; }
        public MessageStatusEnum[] Statuses { get; }
    }
}