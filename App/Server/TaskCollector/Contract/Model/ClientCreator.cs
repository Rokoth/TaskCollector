using System;

namespace TaskCollector.Contract.Model
{
    public class ClientCreator
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string MapRules { get; set; }
    }

    public class ClientUpdater: ClientCreator
    {
        public Guid Id { get; set; }
        public bool PasswordChanged { get; set; }
    }

}
