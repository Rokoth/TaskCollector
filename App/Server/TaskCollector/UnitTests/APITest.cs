//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0

//ref2
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TaskCollector.Controllers;
using TaskCollector.Db.Context;
using Xunit;

namespace TaskCollector.UnitTests
{
    /// <summary>
    /// api unit tests
    /// </summary>
    public class APITest : IClassFixture<CustomFixture>
    {
        private readonly IServiceProvider _serviceProvider;

        public APITest(CustomFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
        }

        /// <summary>
        /// ClientController. Test for Auth method (positive scenario)
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ClientControllerAuthTest()
        {
            var user = await AddUser();
            var client = await AddClient(user.Id);

            await AuthAndAssert(client);
        }

        /// <summary>
        /// MessageController. Test for Send method (positive scenario)
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task MessageControllerSendTest()
        {
            var user = await AddUser();
            var client = await AddClient(user.Id);

            await AuthAndAssert(client);

            var messageController = new MessageApiController(_serviceProvider);
            var messageResult = await messageController.Send(new Dictionary<string, object>()
            {
                { "AddField1", 1 }, { "AddField2", "test_field" }
            });
            Assert.NotNull(messageResult);

            var context = _serviceProvider.GetRequiredService<DbPgContext>();
            var savedMessage = context.Messages.FirstOrDefault();
            Assert.NotNull(savedMessage);
        }

        private async Task AuthAndAssert(Db.Model.Client client)
        {
            var clientController = new ClientApiController(_serviceProvider);
            var result = await clientController.Auth(new Contract.Model.ClientIdentity()
            {
                Login = client.Login,
                Password = $"client_password_{client.Id}"
            });
            var response = result as OkObjectResult;
            Assert.NotNull(response);
            JObject value = JObject.FromObject(response.Value);
            Assert.Equal(client.Id.ToString(), value["username"].ToString());
        }

        private async Task<Db.Model.Client> AddClient(Guid userId)
        {
            var context = _serviceProvider.GetRequiredService<DbPgContext>();
            var client = CreateClient(userId);
            context.Clients.Add(client);
            await context.SaveChangesAsync();
            return client;
        }

        private async Task<Db.Model.User> AddUser()
        {
            var context = _serviceProvider.GetRequiredService<DbPgContext>();
            var user = CreateUser();
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }
        private static Db.Model.Client CreateClient(Guid user_id)
        {
            var client_id = Guid.NewGuid();
            return new Db.Model.Client()
            {
                Id = client_id,
                Description = $"client_description_{client_id}",
                IsDeleted = false,
                Login = $"client_{client_id}",
                Name = $"client_{client_id}",
                Password = SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes($"client_password_{client_id}")),
                VersionDate = DateTimeOffset.Now,
                UserId = user_id
            };
        }

        private Db.Model.User CreateUser()
        {
            var user_id = Guid.NewGuid();
            return new Db.Model.User()
            {
                Name = $"user_{user_id}",
                Id = user_id,
                Description = $"user_description_{user_id}",
                IsDeleted = false,
                Login = $"user_login_{user_id}",
                Password = SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes($"user_password_{user_id}")),
                VersionDate = DateTimeOffset.Now
            };
        }
    }
}
