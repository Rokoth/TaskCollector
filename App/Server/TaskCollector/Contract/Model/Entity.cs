namespace TaskCollector.Contract.Model
{
    public class Entity
    {
    }

    public class UserFilter : Filter<User>
    {
        public int Size { get; set; }
        public int Page { get; set; }
        public string Name { get; set; }
    }

    public abstract class Filter<T> where T : Entity
    { 
    
    }
}