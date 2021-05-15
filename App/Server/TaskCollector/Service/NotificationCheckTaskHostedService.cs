using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Common;
using TaskCollector.Db.Interface;
using TaskCollector.Db.Model;

namespace TaskCollector.Service
{
    public class NotificationCheckTaskHostedService : IHostedService
    {
        private IServiceProvider _serviceProvider;
        private IOptions<NotifyOptions> _options;
        
        public NotificationCheckTaskHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _options = _serviceProvider.GetRequiredService<IOptions<NotifyOptions>>();
        }

        private async Task Run()
        {
            var _messageDataService = _serviceProvider.GetRequiredService<IRepository<Message>>();
            var _messageStatusDataService = _serviceProvider.GetRequiredService<IRepository<MessageStatus>>();
            
            foreach (var notifyRule in _options.Value.NotifyRules)
            {
                Contract.Model.MessageStatusEnum statusEnum;
                if (Enum.TryParse<Contract.Model.MessageStatusEnum>(notifyRule.Name, true, out statusEnum))
                {
                    var cancelTokenSrc = new CancellationTokenSource(60000);
                    var statuses = await _messageStatusDataService.GetAsync(new Filter<MessageStatus>() { 
                       Page = 0,
                       Size = 100,
                       Sort = "StatusDate",
                       Selector = s=> s.IsLast && (s.NextNotifyDate==null || s.NextNotifyDate <= DateTimeOffset.Now)
                    }, cancelTokenSrc.Token);

                    foreach (var status in statuses.Data)
                    {
                        var toNotify = false;
                        if (status.NextNotifyDate == null)
                        {
                            var nextNotify = status.StatusDate.AddHours(notifyRule.FirstTimeNotify);
                            if (nextNotify <= DateTimeOffset.Now)
                            {
                                toNotify = true;
                            }
                        }
                        else
                        { 
                        
                        }
                    }
                }
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
