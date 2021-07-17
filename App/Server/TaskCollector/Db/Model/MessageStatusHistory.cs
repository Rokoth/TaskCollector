using System;
using TaskCollector.Db.Attributes;

namespace TaskCollector.Db.Model
{
    [TableName("h_message_status")]
    public class MessageStatusHistory : EntityHistory
    {
        [ColumnName("message_id")]
        public Guid MessageId { get; set; }
        [ColumnName("status_id")]
        public Contract.Model.MessageStatusEnum StatusId { get; set; }
        [ColumnName("userid")]
        public Guid UserId { get; set; }
        [ColumnName("description")]
        public string Description { get; set; }
        [ColumnName("is_last")]
        public bool IsLast { get; set; }
        [ColumnName("status_date")]
        public DateTimeOffset StatusDate { get; set; }
        [ColumnName("next_notify_date")]
        public DateTimeOffset? NextNotifyDate { get; set; }

    }
}
