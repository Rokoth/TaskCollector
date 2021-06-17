//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref2
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Db.Context;
using TaskCollector.Service;
using Xunit;

namespace TaskCollector.UnitTests
{
    public class NotifyServiceTest : IClassFixture<CustomFixture>
    {
        private readonly IServiceProvider _serviceProvider;

        public NotifyServiceTest(CustomFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
        }

        [Fact]
        public async Task NotifyTest()
        {
            var notifyService = _serviceProvider.GetRequiredService<INotifyService>();
            var context = _serviceProvider.GetRequiredService<DbPgContext>();
           
            var id = Guid.NewGuid();
            var user = new Db.Model.User()
            {
                Name = $"user_name_{id}",
                Id = id,
                Description = $"user_description_{id}",
                IsDeleted = false,
                Login = $"user_login_{id}",
                Password = SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes($"user_password_{id}")),
                Email = "dmitriy.milyaev@mail.ru",
                VersionDate = DateTimeOffset.Now
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();
            await notifyService.Notify(id, new Contract.Model.Notification() { 
               AddFields = "AddFields",
               Client = "Client",
               CreatedDate = DateTimeOffset.Now,
               Description = "Description",
               FeedbackContact = "FeedbackContact",
               Level = "Level",
               NotificationTypeEnum = Contract.Model.NotificationTypeEnum.MessageReceived,
               Title = "Title"
            }, CancellationToken.None);
            Assert.True(true);
        }
    }
}
