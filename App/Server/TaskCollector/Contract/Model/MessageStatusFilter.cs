using System;

namespace TaskCollector.Contract.Model
{
    public class MessageStatusFilter : Filter<MessageStatus>
    {
        public MessageStatusFilter(Guid? messageId, int size, int page, string sort, MessageStatusEnum[] statuses)
        {            
            MessageId = messageId;
            Statuses = statuses;
            Page = page;
            Sort = sort;
            Size = size;
        }
       
        public Guid? MessageId { get; }
        public MessageStatusEnum[] Statuses { get; }
    }
}