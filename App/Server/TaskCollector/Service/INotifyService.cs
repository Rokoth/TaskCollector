using System;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Contract.Model;

namespace TaskCollector.Service
{
    public interface INotifyService
    {
        Task Notify(Guid userId, Notification notification, CancellationToken token);
    }
}