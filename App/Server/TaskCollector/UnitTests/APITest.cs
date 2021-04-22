using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Controllers;
using TaskCollector.Db.Context;
using TaskCollector.Db.Interface;
using TaskCollector.Service;
using Xunit;

namespace TaskCollector.UnitTests
{
    public class APITest : IClassFixture<CustomFixture>
    {
        private readonly IServiceProvider _serviceProvider;

        public APITest(CustomFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
        }

        [Fact]
        public async Task ClientControllerAuthTest()
        {
            var context = _serviceProvider.GetRequiredService<DbPgContext>();

            var user_id = Guid.NewGuid();
            var client_id = Guid.NewGuid();
            context.Users.Add(new Db.Model.User()
            {
                Name = $"user_{user_id}",
                Id = user_id,
                Description = $"user_description_{user_id}",
                IsDeleted = false,
                Login = $"user_login_{user_id}",
                Password = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes($"user_password_{user_id}")),
                VersionDate = DateTimeOffset.Now
            });
            await context.SaveChangesAsync();

            context.Clients.Add(new Db.Model.Client() { 
               Id = client_id,
               Description = $"client_description_{client_id}",
               IsDeleted = false,
               Login = $"client_{client_id}",
               Name = $"client_{client_id}",
               Password = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes($"client_password_{client_id}")),
               VersionDate = DateTimeOffset.Now,
               UserId = user_id
            });
            await context.SaveChangesAsync();

            var clientController = new ClientApiController(_serviceProvider);

            var result = await clientController.Auth(new Contract.Model.ClientIdentity() { 
               Login = $"client_{client_id}",
               Password = $"client_password_{client_id}"
            });

            Assert.NotNull(result);
            Assert.Equal(client_id.ToString(), clientController.User.Identity.Name);
        }

        [Fact]
        public async Task MessageControllerSendTest()
        {
            var context = _serviceProvider.GetRequiredService<DbPgContext>();

            var user_id = Guid.NewGuid();
            var client_id = Guid.NewGuid();
            context.Users.Add(new Db.Model.User()
            {
                Name = $"user_{user_id}",
                Id = user_id,
                Description = $"user_description_{user_id}",
                IsDeleted = false,
                Login = $"user_login_{user_id}",
                Password = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes($"user_password_{user_id}")),
                VersionDate = DateTimeOffset.Now
            });
            await context.SaveChangesAsync();

            context.Clients.Add(new Db.Model.Client()
            {
                Id = client_id,
                Description = $"client_description_{client_id}",
                IsDeleted = false,
                Login = $"client_{client_id}",
                Name = $"client_{client_id}",
                Password = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes($"client_password_{client_id}")),
                VersionDate = DateTimeOffset.Now,
                UserId = user_id
            });
            await context.SaveChangesAsync();

            var clientController = new ClientApiController(_serviceProvider);

            var result = await clientController.Auth(new Contract.Model.ClientIdentity()
            {
                Login = $"client_{client_id}",
                Password = $"client_password_{client_id}"
            });

            Assert.NotNull(result);
            Assert.Equal(client_id.ToString(), clientController.User.Identity.Name);

            var messageController = new MessageApiController(_serviceProvider);
            var messageResult = await messageController.Send(new Dictionary<string, object>()
            {
                { "AddField1", 1 }, { "AddField2", "test_field" }
            });
            Assert.NotNull(messageResult);

            var savedMessage = context.Messages.FirstOrDefault();
            Assert.NotNull(savedMessage);
        }




    }
}
