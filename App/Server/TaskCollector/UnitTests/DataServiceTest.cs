//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref2
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Db.Context;
using TaskCollector.Service;
using Xunit;

namespace TaskCollector.UnitTests
{

    public class DataServiceTest : IClassFixture<CustomFixture>
    {
        private readonly IServiceProvider _serviceProvider;

        public DataServiceTest(CustomFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
        }

        /// <summary>
        /// GetUsers positive test
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetUsersTest()
        {
            var context = _serviceProvider.GetRequiredService<DbPgContext>();

            List<Db.Model.User> users = new List<Db.Model.User>();
            AddUsers(users, 10, "user_select_{0}");
            AddUsers(users, 10, "user_not_select_{0}");
            foreach (var user in users) context.Users.Add(user);
            await context.SaveChangesAsync();

            var dataService = _serviceProvider.GetRequiredService<IGetDataService<Contract.Model.User, Contract.Model.UserFilter>>();
            var data = await dataService.GetAsync(
                new Contract.Model.UserFilter(10, 0, null, "user_select", null), CancellationToken.None);

            Assert.Equal(10, data.Data.Count());
            foreach (var item in data.Data)
            {
                Assert.Contains("user_select", item.Name);
            }
        }

        /// <summary>
        /// GetUser positive test
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetUserTest()
        {
            var context = _serviceProvider.GetRequiredService<DbPgContext>();

            List<Db.Model.User> users = new List<Db.Model.User>();
            AddUsers(users, 10, "user_{0}");
            
            foreach (var user in users) context.Users.Add(user);
            await context.SaveChangesAsync();

            var actuser = users.First();
            var dataService = _serviceProvider.GetRequiredService<IGetDataService<Contract.Model.User, Contract.Model.UserFilter>>();
            var data = await dataService.GetAsync(actuser.Id, CancellationToken.None);

            Assert.NotNull(data);
            Assert.Equal($"user_{actuser.Id}", data.Name);
        }

        /// <summary>
        /// AddUser positive test
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddUserTest()
        {
            var context = _serviceProvider.GetRequiredService<DbPgContext>();

            var id = Guid.NewGuid();
            var user = new Contract.Model.UserCreator()
            {
                Name = string.Format("user_{0}", id),
                Description = string.Format("user_description_{0}", id),                
                Login = string.Format("user_login_{0}", id),
                Password = string.Format("user_password_{0}", id)
            };            
                                    
            var dataService = _serviceProvider.GetRequiredService<IAddDataService<Contract.Model.User, Contract.Model.UserCreator>>();
            await dataService.AddAsync(user, CancellationToken.None);
            var checkUser = context.Users.FirstOrDefault();
            Assert.NotNull(checkUser);
            Assert.Equal($"user_{id}", checkUser.Name);
            Assert.Equal($"user_description_{id}", checkUser.Description);
            Assert.Equal($"user_login_{id}", checkUser.Login);
        }

        /// <summary>
        /// GetClients positive test
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetClientsTest()
        {
            var context = _serviceProvider.GetRequiredService<DbPgContext>();
            var userId = Guid.NewGuid();
            var user = new Db.Model.User()
            {
                Name = string.Format("user_{0}", userId),
                Id = userId,
                Description = string.Format("user_description_{0}", userId),
                IsDeleted = false,
                Login = string.Format("user_login_{0}", userId),
                Password = SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(string.Format("user_password_{0}", userId))),
                VersionDate = DateTimeOffset.Now
            };            
            context.Users.Add(user);
            await context.SaveChangesAsync();

            List<Db.Model.Client> clients = new List<Db.Model.Client>();
            AddClients(clients, userId, 10, "client_select_{0}");
            AddClients(clients, userId, 10, "client_not_select_{0}");
            foreach (var client in clients) context.Clients.Add(client);
            await context.SaveChangesAsync();

            var dataService = _serviceProvider.GetRequiredService<IGetDataService<Contract.Model.Client, Contract.Model.ClientFilter>>();
            var data = await dataService.GetAsync(
                new Contract.Model.ClientFilter(10, 0, null, "client_select", null, userId), CancellationToken.None);

            Assert.Equal(10, data.Data.Count());
            foreach (var item in data.Data)
            {
                Assert.Contains("client_select", item.Name);
            }
        }

        private void AddUsers(List<Db.Model.User> users, int count, string nameMask)
        {
            for (int i = 0; i < count; i++)
            {
                AddUser(users, nameMask, "user_description_{0}", "user_login_{0}", "user_password_{0}");
            }
        }

        private void AddUser(List<Db.Model.User> users, string nameMask, string descriptionMask, string loginMask, string passwordMask)
        {
            var id = Guid.NewGuid();
            users.Add(new Db.Model.User()
            {
                Name = string.Format(nameMask, id),
                Id = id,
                Description = string.Format(descriptionMask, id),
                IsDeleted = false,
                Login = string.Format(loginMask, id),
                Password = SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(string.Format(passwordMask, id))),
                VersionDate = DateTimeOffset.Now
            });
        }

        private void AddClients(List<Db.Model.Client> clients, Guid userId, int count, string nameMask)
        {
            for (int i = 0; i < count; i++)
            {
                AddClient(clients, userId, nameMask, "client_description_{0}", "client_login_{0}", "client_password_{0}");
            }
        }

        private void AddClient(List<Db.Model.Client> clients, Guid userId, string nameMask, string descriptionMask, string loginMask, string passwordMask)
        {
            var id = Guid.NewGuid();
            clients.Add(new Db.Model.Client()
            {
                Name = string.Format(nameMask, id),
                Id = id,
                UserId = userId,
                MapRules = "{}",
                Description = string.Format(descriptionMask, id),
                IsDeleted = false,
                Login = string.Format(loginMask, id),
                Password = SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(string.Format(passwordMask, id))),
                VersionDate = DateTimeOffset.Now
            });
        }
    }
}
