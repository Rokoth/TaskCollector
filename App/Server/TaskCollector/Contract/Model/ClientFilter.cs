namespace TaskCollector.Contract.Model
{
    public class ClientFilter : Filter<Client>
    {
        public ClientFilter(int size, int page, string sort, string name) : base(size, page, sort)
        {
            Name = name;
        }
        public string Name { get; }
    }
}