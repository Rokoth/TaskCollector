using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private INotifyService _notifyService;
        private ILogger _logger;
        private CancellationTokenSource _tokenSource;

        public NotificationCheckTaskHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _options = _serviceProvider.GetRequiredService<IOptions<NotifyOptions>>();
            _notifyService = _serviceProvider.GetRequiredService<INotifyService>();
            _tokenSource = new CancellationTokenSource();
        }

        private async Task Run(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var _messageRepo = _serviceProvider.GetRequiredService<IRepository<Message>>();
                var _clientRepo = _serviceProvider.GetRequiredService<IRepository<Client>>();
                var _userRepo = _serviceProvider.GetRequiredService<IRepository<User>>();
                var _messageStatusRepo = _serviceProvider.GetRequiredService<IRepository<MessageStatus>>();

                try
                {
                    foreach (var notifyRule in _options.Value.NotifyRules)
                    {
                        if (Enum.TryParse(notifyRule.Name, true, out Contract.Model.MessageStatusEnum statusEnum))
                        {
                            var cancelTokenSrc = new CancellationTokenSource(60000);
                            var statuses = await _messageStatusRepo.GetAsync(new Filter<MessageStatus>()
                            {
                                Page = 0,
                                Size = 100,
                                Sort = "StatusDate",
                                Selector = s => s.IsLast && s.StatusId == statusEnum && s.NextNotifyDate == null
                            }, cancelTokenSrc.Token);

                            var level = notifyRule.Levels.First(s => s.Order == 1);

                            foreach (var status in statuses.Data)
                            {

                                var nextNotify = status.StatusDate.AddHours(level.FirstTimeNotify);
                                if (nextNotify > DateTimeOffset.Now)
                                {
                                    status.NextNotifyDate = nextNotify;
                                    await _messageStatusRepo.UpdateAsync(status, true, cancelTokenSrc.Token);
                                    continue;
                                }

                                await Notify(_messageRepo, _clientRepo, cancelTokenSrc, level, status);

                                status.NextNotifyDate = nextNotify.AddHours(level.RepeatInterval);
                                await _messageStatusRepo.UpdateAsync(status, true, cancelTokenSrc.Token);
                            }

                            statuses = await _messageStatusRepo.GetAsync(new Filter<MessageStatus>()
                            {
                                Page = 0,
                                Size = 100,
                                Sort = "StatusDate",
                                Selector = s => s.IsLast && s.StatusId == statusEnum && s.NextNotifyDate < DateTimeOffset.Now
                            }, cancelTokenSrc.Token);

                            foreach (var status in statuses.Data)
                            {
                                level = null;
                                foreach (var lev in notifyRule.Levels.OrderByDescending(s => s.Order))
                                {
                                    if (status.StatusDate.AddHours(lev.FirstTimeNotify) < DateTimeOffset.Now)
                                    {
                                        level = lev;
                                        break;
                                    }
                                }
                                await Notify(_messageRepo, _clientRepo, cancelTokenSrc, level, status);

                                status.NextNotifyDate = status.NextNotifyDate.Value.AddHours(level.RepeatInterval);
                                await _messageStatusRepo.UpdateAsync(status, true, cancelTokenSrc.Token);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error in NotificationCheckTaskHostedService: {ex.Message} {ex.StackTrace}");                    
                }
            }
        }

        private async Task Notify(IRepository<Message> _messageRepo, IRepository<Client> _clientRepo, 
            CancellationTokenSource cancelTokenSrc, NotifyLevel level, MessageStatus status)
        {
            var message = await _messageRepo.GetAsync(status.MessageId, cancelTokenSrc.Token);
            var client = await _clientRepo.GetAsync(message.ClientId, cancelTokenSrc.Token);
            await _notifyService.Notify(client.UserId, new Contract.Model.Notification()
            {
                AddFields = message.AddFields,
                Client = client.Name,
                CreatedDate = message.CreatedDate,
                Description = message.Description,
                FeedbackContact = message.FeedbackContact,
                Level = ((Contract.Model.MessageLevelEnum)message.Level).ToString(),
                NotificationTypeEnum = (Contract.Model.NotificationTypeEnum)level.NotificationTypeEnum,
                Title = "Автоматическое уведомление"
            }, cancelTokenSrc.Token);
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Factory.StartNew(()=>  Run(_tokenSource.Token), cancellationToken,
                TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _tokenSource.Cancel();
        }
    }
}
