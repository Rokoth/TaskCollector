using System;
using System.Collections.Generic;
using System.Text;

namespace TaskCollector.Contract.Model
{
    public class User: Entity
    {

    }

    public class UserCreator {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class Client : Entity
    {
        public string MapRules { get; set; }
    }

    public class Message : Entity
    {
        
    }

    public class MessageCreator
    {
        public Guid ClientId { get; set; }
        public string AddFields { get; set; }
    }

    public class MessageUpdater
    {

    }

    public class ClientIdentity
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class UserIdentity
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

}
