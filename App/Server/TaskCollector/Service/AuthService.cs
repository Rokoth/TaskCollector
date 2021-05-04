using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskCollector.Service
{
    public class AuthService: IAuthService
    {
        private IServiceProvider _serviceProvider;
        private IMapper _mapper;
        public AuthService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _mapper = _serviceProvider.GetRequiredService<IMapper>();
        }                    

        public async Task<ClaimsIdentity> Auth(Contract.Model.ClientIdentity login, CancellationToken token)
        {
            var repo = _serviceProvider.GetRequiredService<Db.Interface.IRepository<Db.Model.Client>>();
            var password = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(login.Password));
            var client = (await repo.GetAsync(new Db.Model.Filter<Db.Model.Client>() { 
              Page = 0,
              Size = 10,
              Selector = s=>s.Login == login.Login && s.Password == password
            }, token)).Data.FirstOrDefault();
            if (client != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, client.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "Client")
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }

        public async Task<ClaimsIdentity> Auth(Contract.Model.UserIdentity login, CancellationToken token)
        {
            var repo = _serviceProvider.GetRequiredService<Db.Interface.IRepository<Db.Model.User>>();
            var password = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(login.Password));
            var user = (await repo.GetAsync(new Db.Model.Filter<Db.Model.User>()
            {
                Page = 0,
                Size = 10,
                Selector = s => s.Login == login.Login && s.Password == password
            }, token)).Data.FirstOrDefault();
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString())
                };             
               ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
               return id;
            }
            return null;
        }        
    }
}
