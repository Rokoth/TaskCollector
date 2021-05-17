//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref2

using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Поле должно быть установлено")]
        [Remote("CheckName", "Client", ErrorMessage = "Имя уже используется")]
        public string Name { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        [Display(Name = "Описание")]
        public string Description { get; set; }
        /// <summary>
        /// Логин
        /// </summary>
        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Поле должно быть установлено")]
        [Remote("CheckLogin", "Client", ErrorMessage = "Логин уже используется")]
        public string Login { get; set; }
                
        /// <summary>
        /// Правила маппинга сообщения
        /// </summary>
        [Display(Name = "Правила маппинга сообщений")]
        public string MapRules { get; set; }
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [Display(Name = "Идентификатор пользователя")]
        public Guid UserId { get; set; }
        /// <summary>
        /// Пользователь
        /// </summary>
        [Display(Name = "Пользователь")]
        public User User { get; set; }
    }

    public class ClientHistory : EntityHistory
    {
        /// <summary>
        /// Наименование
        /// </summary>
        [Display(Name = "Имя")]        
        public string Name { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        [Display(Name = "Описание")]
        public string Description { get; set; }
        /// <summary>
        /// Логин
        /// </summary>
        [Display(Name = "Логин")]
       public string Login { get; set; }

        /// <summary>
        /// Правила маппинга сообщения
        /// </summary>
        [Display(Name = "Правила маппинга сообщений")]
        public string MapRules { get; set; }
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [Display(Name = "Идентификатор пользователя")]
        public Guid UserId { get; set; }
        /// <summary>
        /// Пользователь
        /// </summary>
        [Display(Name = "Пользователь")]
        public User User { get; set; }
    }
}
