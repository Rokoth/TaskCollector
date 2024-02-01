using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Common;
using TaskCollector.Contract.Model;

namespace TaskCollector.Service
{
    public class NotifyService : INotifyService
    {
        private IServiceProvider _serviceProvider;
        private ILogger _logger;
        private NotifyCredentials _notifyOptions;

        public NotifyService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = _serviceProvider.GetRequiredService<ILogger<NotifyService>>();
            var _options = _serviceProvider.GetRequiredService<IOptions<NotifyOptions>>().Value;
            _notifyOptions = _options.Credentials;
        }

        public async Task Notify(Guid userId, Notification notification, CancellationToken token)
        {
            try
            {
                var userDataService = _serviceProvider.GetRequiredService<IGetDataService<User, UserFilter>>();
                var user = await userDataService.GetAsync(userId, userId, token);
                if (user != null)
                {
                    var email = user.Email;
                    string text = $"Поступило новое уведомление от системы {notification.Client}:\r\n" +
                        $"{notification.NotificationType}\r\n" +
                        $"Заголовок: {notification.Title}\r\n" +
                        $"Описание: {notification.Description}\r\n" +
                        $"Уровень: {notification.Level}\r\n" +
                        $"Обратная связь: {notification.FeedbackContact}\r\n" +
                        $"Дополнительно: {notification.AddFields}\r\n" +
                        $"Создано: {notification.CreatedDate}\r\n";
                    await SendEmailAsync(email, text);
                }
                else
                {
                    throw new NotificationException("Пользователь не найден");
                }
            }
            catch (NotificationException ex)
            {
                _logger.LogError($"Ошибка при отправке уведомления: {ex.Message} {ex.StackTrace}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при отправке уведомления: {ex.Message} {ex.StackTrace}");
                throw new NotificationException($"Ошибка при отправке уведомления: {ex.Message}");
            }
        }

        private async Task SendEmailAsync(string toEmail, string text)
        {
            MailAddress from = new MailAddress(_notifyOptions.FromEmail, _notifyOptions.FromName);
            MailAddress to = new MailAddress(toEmail);
            MailMessage m = new MailMessage(from, to)
            {
                Subject = $"Уведомление системы {_notifyOptions.FromName}",
                Body = text
            };

            //SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            SmtpClient smtp = new SmtpClient(_notifyOptions.MailServer, _notifyOptions.Port)
            {
                Credentials = new NetworkCredential(_notifyOptions.FromEmail, _notifyOptions.FromPassword),
                EnableSsl = true                
            };
            await smtp.SendMailAsync(m);
        }
    }
}
