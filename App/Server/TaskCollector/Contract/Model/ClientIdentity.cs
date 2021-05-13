//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref 2
namespace TaskCollector.Contract.Model
{
    /// <summary>
    /// Модель авторизации клиента
    /// </summary>
    public class ClientIdentity : IIdentity
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
    }
}
