namespace TaskCollector.Contract.Model
{
    public class MessageFilter : Filter<Message>
    {
        public MessageFilter(int size, int page, string sort, string title) : base(size, page, sort)
        {
            Title = title;
        }
        public string Title { get; }
    }
}