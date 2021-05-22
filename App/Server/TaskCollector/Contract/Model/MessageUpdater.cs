using System;

namespace TaskCollector.Contract.Model
{
    public class MessageUpdater : IEntity
    {
        public Guid Id { get; set; }
    }

    public class MessageStatusUpdater : IEntity
    {
        public Guid Id { get; set; }
    }

    public class MessageStatusCreator
    {

    }

}
