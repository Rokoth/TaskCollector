using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using TaskCollector.Db.Attributes;

namespace TaskCollector.Db.Model
{
    [TableName("user")]
    public class User : Entity
    {
        [ColumnName("name")]
        public string Name { get; set; }
        [ColumnName("description")]
        public string Description { get; set; }
        [ColumnName("login")]
        public string Login { get; set; }
        [ColumnName("password")]
        public byte[] Password { get; set; }        
    }

    public class Client : Entity
    {
        [ColumnName("name")]
        public string Name { get; set; }
        [ColumnName("description")]
        public string Description { get; set; }
        [ColumnName("login")]
        public string Login { get; set; }
        [ColumnName("password")]
        public byte[] Password { get; set; }
        [ColumnType("json")]
        [ColumnName("mapping_rules")]
        public string MappingRules { get; set; }
    }

    public class MessageStatus : Entity
    {
        [ColumnName("name")]
        public string Name { get; set; }
    }


    public class ClientIdentity : IdentityUser
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class UserIdentity : IdentityUser
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
