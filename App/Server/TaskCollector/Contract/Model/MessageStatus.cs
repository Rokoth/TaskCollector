using System;

namespace TaskCollector.Contract.Model
{
    public enum MessageStatusEnum
    { 
        New = 0,
        Working = 1,
        Deferred = 2,
        Ready = 3,
        Closed = 100
    }

    public class MessageStatus : Entity
    {
        public Guid MessageId { get; set; }
        public MessageStatusEnum StatusId { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; }
        public bool IsLast { get; set; }
        public DateTimeOffset StatusDate { get; set; }
        public DateTimeOffset? NextNotifyDate { get; set; }

    }

}
