using System;

namespace TaskCollector.Contract.Model
{
    public class UserUpdater
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool PasswordChanged { get; set; }
    }

}
