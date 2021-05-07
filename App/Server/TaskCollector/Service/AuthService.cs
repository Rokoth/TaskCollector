//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref2
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
    /// <summary>
    /// Wrapper for auth methods
    /// </summary>
    public class AuthService: IAuthService
    {
        private const string CLIENT_ROLE_TYPE = "Client";
        private const string TOKEN_AUTH_TYPE = "Token";
        private const string USER_ROLE_TYPE = "User";
        private const string COOKIES_AUTH_TYPE = "Cookies";

        private readonly IServiceProvider _serviceProvider;        
        public AuthService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;           
        }                    

        /// <summary>
        /// Client Auth
        /// </summary>
        /// <param name="login"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> Auth(Contract.Model.ClientIdentity login, CancellationToken token)
        {
            return await AuthInternal<Db.Model.Client, Contract.Model.ClientIdentity>(login, CLIENT_ROLE_TYPE, TOKEN_AUTH_TYPE,  token);
        }

        /// <summary>
        /// User Auth
        /// </summary>
        /// <param name="login"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> Auth(Contract.Model.UserIdentity login, CancellationToken token)
        {
            return await AuthInternal<Db.Model.User, Contract.Model.UserIdentity>(login, USER_ROLE_TYPE, COOKIES_AUTH_TYPE, token);            
        }

        private async Task<ClaimsIdentity> AuthInternal<T, I>(I login, string roleType, string authType, CancellationToken token)
            where T: Db.Model.Entity, Db.Model.IIdentity
            where I: Contract.Model.IIdentity
        {
            var repo = _serviceProvider.GetRequiredService<Db.Interface.IRepository<T>>();
            var password = SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(login.Password));
            var client = (await repo.GetAsync(new Db.Model.Filter<T>()
            {
                Page = 0,
                Size = 10,
                Selector = s => s.Login == login.Login && s.Password == password
            }, token)).Data.FirstOrDefault();
            if (client != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, client.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, roleType)
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, authType, 
                    ClaimsIdentity.DefaultNameClaimType, 
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            // если пользователя/клиента не найдено
            return null;
        }            
    }
}
