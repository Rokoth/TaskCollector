using System;
using System.Linq.Expressions;
using TaskCollector.Db.Attributes;

namespace TaskCollector.Db.Model
{
    public abstract class Entity
    {
        [PrimaryKey]
        [ColumnName("id")]
        public Guid Id { get; set; }
        [ColumnName("version_date")]
        public DateTimeOffset VersionDate { get; set; }
        [ColumnName("is_deleted")]
        public bool IsDeleted { get; set; }
    }

    public class UserFilter : Filter<User>
    {
        
    }

    public class MessageFilter : Filter<Message>
    {

    }

    public abstract class Filter<T> where T : Entity
    {
        public int Page;
        public int Size { get; set; }
        public Expression<Func<T, bool>> Selector { get; set; }
    }
}