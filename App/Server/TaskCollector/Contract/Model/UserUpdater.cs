using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace TaskCollector.Contract.Model
{
    public class UserUpdater: IEntity
    {
        [Display(Name = "ИД")]
        public Guid Id { get; set; }
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Поле должно быть установлено")]
        [Remote("CheckNameEdit", "User", ErrorMessage = "Имя уже используется", AdditionalFields ="Id")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Поле должно быть установлено")]
        [Remote("CheckLoginEdit", "User", ErrorMessage = "Логин уже используется", AdditionalFields = "Id")]
        public string Login { get; set; }
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Пароль изменен")]
        public bool PasswordChanged { get; set; }
    }

}
