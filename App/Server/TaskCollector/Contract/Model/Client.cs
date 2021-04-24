//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref2

using System;

namespace TaskCollector.Contract.Model
{
    /// <summary>
    /// Клиент (клиентская модель)
    /// </summary>
    public class Client : Entity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }        
        /// <summary>
        /// Правила маппинга сообщения
        /// </summary>
        public string MapRules { get; set; }
        /// <summary>
        /// Пользователь
        /// </summary>
        public Guid UserId { get; set; }
    }
}
