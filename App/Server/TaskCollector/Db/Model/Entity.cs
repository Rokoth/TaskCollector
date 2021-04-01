using System;
using System.Linq.Expressions;

namespace TaskCollector.Db.Model
{
    public class Entity
    {
    }

    public class UserFilter : Filter<User>
    {
        
    }

    public abstract class Filter<T> where T : Entity
    {
        public int Page;
        public int Size { get; set; }
        public Expression<Func<T, bool>> Selector { get; set; }
    }
}