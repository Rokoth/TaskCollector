using System;
using TaskCollector.Db.Attributes;

namespace TaskCollector.Db.Model
{
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
}
