using System;

namespace TaskCollector.Contract.Model
{
    public interface IIdentity
    {
        Guid Id { get; set; }
    }
}