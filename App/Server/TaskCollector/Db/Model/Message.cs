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

    public class MessageStatus : Entity
    {
        public Guid MessageId { get; set; }
        public Contract.Model.MessageStatusEnum StatusId { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; }
        public bool IsLast { get; set; }
        public DateTimeOffset StatusDate { get; set; }
        public DateTimeOffset? NextNotifyDate { get; set; }

    }
}
