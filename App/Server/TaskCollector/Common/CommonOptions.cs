//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref2

using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Text;

namespace TaskCollector.Common
{
    /// <summary>
    /// Класс хранения настроек
    /// </summary>
    public class CommonOptions
    {        
        /// <summary>
        /// Строка полключения к базе данных
        /// </summary>
        public string ConnectionString { get; set; }        
    }

    public class NotifyOptions
    { 
       public List<NotifyRule> NotifyRules { get; set; }
    }

    public class NotifyRule
    { 
        public string Name { get; set; }
        public int FirstTimeNotify { get; set; }
        public int RepeatInterval { get; set; }
    }

    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // издатель токена
        public const string AUDIENCE = "MyAuthClient"; // потребитель токена
        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
        public const int LIFETIME = 5; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
