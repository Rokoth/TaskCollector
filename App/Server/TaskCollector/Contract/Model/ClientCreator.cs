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
    /// Class for creating client
    /// </summary>
    public class ClientCreator
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
        /// Пароль
        /// </summary>
        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Поле должно быть установлено")]
        [DataType(DataType.Password)]
        public string Password { get; set; }        
    }
}
