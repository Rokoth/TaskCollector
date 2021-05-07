//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0

//ref2
using System;

namespace TaskCollector.Db.Attributes
{
    public class ColumnTypeAttribute : Attribute
    {
        public string Name { get; }

        public ColumnTypeAttribute(string name)
        {
            Name = name;
        }
    }
}
