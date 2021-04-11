﻿using System;
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
        public string Password { get; set; }        
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
        public string Password { get; set; }
        [ColumnType("json")]
        [ColumnName("mapping_rules")]
        public string MappingRules { get; set; }
    }

    public class Message : Entity
    {
        [ColumnName("name")]
        public string Name { get; set; }
    }

    public class MessageStatus : Entity
    {
        [ColumnName("name")]
        public string Name { get; set; }
    }

    public class Settings : Entity
    {
        [ColumnName("param_name")]
        public string ParamName { get; set; }

        [ColumnName("param_value")]
        public string ParamValue { get; set; }

        
    }
}
