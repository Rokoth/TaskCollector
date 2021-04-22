using System;
using System.Linq.Expressions;

namespace TaskCollector.Db.Model
{
    public class Filter<T> where T : Entity
    {        
        public int Page { get; set; }
        public int Size { get; set; }
        public string Sort { get; set; }

        public Expression<Func<T, bool>> Selector { get; set; }
    }
}