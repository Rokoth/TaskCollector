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
                new Contract.Model.UserFilter(10, 0, null, "user_select"), CancellationToken.None);

            Assert.Equal(10, data.Data.Count());
            foreach (var item in data.Data)
            {
                Assert.Contains("user_select", item.Name);
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
                Password = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(string.Format(passwordMask, id))),
                VersionDate = DateTimeOffset.Now
            });
        }
    }
}
