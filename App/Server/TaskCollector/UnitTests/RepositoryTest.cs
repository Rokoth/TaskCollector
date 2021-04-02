using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Db.Context;
using TaskCollector.Db.Interface;
using TaskCollector.Db.Model;
using Xunit;

namespace TaskCollector.UnitTests
{
    public class RepositoryTest : IClassFixture<CustomFixture>
    {
        private readonly IServiceProvider _serviceProvider;

        public RepositoryTest(CustomFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
        }

        [Fact]
        public async Task GetTest()
        {
            var context = _serviceProvider.GetRequiredService<DbPgContext>();
            for (int i = 0; i < 10; i++)
            {
                var id = Guid.NewGuid();
                context.Users.Add(new Db.Model.User() { 
                   Name = $"user_select_{id}",
                   Id = id,
                   Description = $"user_description_{id}",
                   IsDeleted = false,
                   Login = $"user_login_{id}",
                   Password = Encoding.UTF8.GetString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes($"user_password_{id}"))),
                   VersionDate = DateTimeOffset.Now
                });
            }

            for (int i = 0; i < 10; i++)
            {
                var id = Guid.NewGuid();
                context.Users.Add(new Db.Model.User()
                {
                    Name = $"user_not_select_{id}",
                    Id = id,
                    Description = $"user_description_{id}",
                    IsDeleted = false,
                    Login = $"user_login_{id}",
                    Password = Encoding.UTF8.GetString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes($"user_password_{id}"))),
                    VersionDate = DateTimeOffset.Now
                });
            }

            await context.SaveChangesAsync();

            var repo = _serviceProvider.GetRequiredService<IRepository<User>>();
            var data = await repo.GetAsync(new UserFilter() { 
                Page = 0, 
                Size = 10, 
                Selector = s=>s.Name.Contains("user_select")
            }, CancellationToken.None);

            Assert.Equal(10, data.Count());
            foreach (var item in data)
            {
                Assert.Contains("user_select", item.Name);
            }
        }
    }
}
