using System;
using System.Collections.Generic;

namespace TaskCollector.Contract.Model
{
    public class MessageFilter : Filter<Message>
    {
        public MessageFilter(int size, int page, string sort, string title, Guid? clientId,
            List<int> levels, DateTimeOffset? dateFrom, DateTimeOffset? dateTo) : base(size, page, sort)
        {
            Title = title;
            ClientId = clientId;
            Levels = levels;
            DateFrom = dateFrom;
            DateTo = dateTo;
        }
        public string Title { get; }
        public Guid? ClientId { get; set; }
        public List<int> Levels { get; set; }
        public DateTimeOffset? DateFrom { get; set; }
        public DateTimeOffset? DateTo { get; set; }
    }
}