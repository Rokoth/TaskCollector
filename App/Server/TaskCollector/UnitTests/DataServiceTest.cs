using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Db.Context;
using TaskCollector.Db.Interface;
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

        [Fact]
        public async Task GetUsersTest()
        {
            var context = _serviceProvider.GetRequiredService<DbPgContext>();

            List<Db.Model.User> users = new List<Db.Model.User>();
            
            for (int i = 0; i < 10; i++)
            {
                var id = Guid.NewGuid();
                users.Add(new Db.Model.User() { 
                   Name = $"user_select_{id}",
                   Id = id,
                   Description = $"user_description_{id}",
                   IsDeleted = false,
                   Login = $"user_login_{id}",
                   Password = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes($"user_password_{id}")),
                   VersionDate = DateTimeOffset.Now
                });
            }

            for (int i = 0; i < 10; i++)
            {
                var id = Guid.NewGuid();
                users.Add(new Db.Model.User()
                {
                    Name = $"user_not_select_{id}",
                    Id = id,
                    Description = $"user_description_{id}",
                    IsDeleted = false,
                    Login = $"user_login_{id}",
                    Password = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes($"user_password_{id}")),
                    VersionDate = DateTimeOffset.Now
                });
            }


            foreach (var user in users)
            {
                context.Users.Add(user);
            }
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

        
        

    }
}
