﻿//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref2
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace TaskCollector.Contract.Model
{
    public class ClientUpdater: IEntity
    {        
        public Guid Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Поле должно быть установлено")]
        [Remote("CheckNameEdit", "Client", ErrorMessage = "Имя уже используется", AdditionalFields = "Id")]
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
        [Remote("CheckLoginEdit", "Client", ErrorMessage = "Логин уже используется", AdditionalFields = "Id")]
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
        /// Пароль
        /// </summary>
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool PasswordChanged { get; set; }

        
    }

}
