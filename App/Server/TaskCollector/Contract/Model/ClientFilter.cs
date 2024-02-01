//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref2
using System;

namespace TaskCollector.Contract.Model
{
    /// <summary>
    /// Filter class for Client model
    /// </summary>
    public class ClientFilter : Filter<Client>
    {        
        public string Name { get; set; }
        public string Login { get; set; }

        public static ClientFilter NameClientFilter(string name)
        {
            return new ClientFilter() { Name = name };
        }

        public static ClientFilter LoginClientFilter(string login)
        {
            return new ClientFilter() { Login = login };
        }
    }

    
}