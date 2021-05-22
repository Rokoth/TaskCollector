using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Contract.Model;

namespace TaskCollector.Service
{
    public class NotifyService : INotifyService
    {
        private IServiceProvider _serviceProvider;
        private ILogger _logger;
        public NotifyService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = _serviceProvider.GetRequiredService<ILogger<NotifyService>>();
        }

        public async Task Notify(Guid userId, Notification notification, CancellationToken token)
        {
            try
            {
                var userDataService = _serviceProvider.GetRequiredService<IGetDataService<User, UserFilter>>();
                var user = await userDataService.GetAsync(userId, token);
                if (user != null)
                {

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

        private static async Task SendEmailAsync()
        {
            MailAddress from = new MailAddress("somemail@gmail.com", "Tom");
            MailAddress to = new MailAddress("somemail@yandex.ru");
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Тест";
            m.Body = "Письмо-тест 2 работы smtp-клиента";
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("somemail@gmail.com", "mypassword");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
            Console.WriteLine("Письмо отправлено");
        }
    }
}
