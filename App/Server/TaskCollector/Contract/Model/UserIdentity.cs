using System.ComponentModel.DataAnnotations;

namespace TaskCollector.Contract.Model
{
    public class UserIdentity
    {
        [Display(Name = "Логин")]
        public string Login { get; set; }
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }

}
