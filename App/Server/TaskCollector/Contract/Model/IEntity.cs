using System;

namespace TaskCollector.Contract.Model
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}