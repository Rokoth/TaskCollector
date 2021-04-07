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

    }

    public class Message : Entity
    {
        
    }

    public class MessageCreator
    {

    }

    public class MessageUpdater
    {

    }
}
