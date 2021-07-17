using System;
using TaskCollector.Db.Attributes;

namespace TaskCollector.Db.Model
{
    [TableName("message")]
    public class Message : Entity
    {
        [ColumnName("level")]
        public int Level { get; set; }
        [ColumnName("title")]
        public string Title { get; set; }
        [ColumnName("description")]
        public string Description { get; set; }
        [ColumnName("feedback_contact")]
        public string FeedbackContact { get; set; }
        [ColumnName("client_id")]
        public Guid ClientId { get; set; }
        [ColumnName("created_date")]
        public DateTimeOffset CreatedDate { get; set; }
        [ColumnName("add_fields")]
        [ColumnType("json")]
        public string AddFields { get; set; }

    }

    [TableName("h_message")]
    public class MessageHistory : EntityHistory
    {
        [ColumnName("level")]
        public int Level { get; set; }
        [ColumnName("title")]
        public string Title { get; set; }
        [ColumnName("description")]
        public string Description { get; set; }
        [ColumnName("feedback_contact")]
        public string FeedbackContact { get; set; }
        [ColumnName("client_id")]
        public Guid ClientId { get; set; }
        [ColumnName("created_date")]
        public DateTimeOffset CreatedDate { get; set; }
        [ColumnName("add_fields")]
        public string AddFields { get; set; }

    }

    [TableName("message_status")]
    public class MessageStatus : Entity
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
