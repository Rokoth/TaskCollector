//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0

//ref2
using System;

namespace TaskCollector.Db.Attributes
{
    /// <summary>
    /// DB Model TableName attribute 
    /// </summary>
    public class TableNameAttribute : Attribute
    {
        /// <summary>
        /// table name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name">table name</param>
        public TableNameAttribute(string name)
        {
            Name = name;
        }
    }
}
