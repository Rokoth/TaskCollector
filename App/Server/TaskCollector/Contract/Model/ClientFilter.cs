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
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="size">page size</param>
        /// <param name="page">page number</param>
        /// <param name="sort">order by</param>
        /// <param name="name">client name</param>
        /// <param name="login">client login</param>
        /// <param name="userId">Id of user (required)</param>
        public ClientFilter(int size, int page, string sort, string name, string login, Guid userId) : base(size, page, sort)
        {
            Name = name;
            Login = login;
            UserId = userId;
        }
        public string Name { get; }
        public string Login { get; }
        public Guid UserId { get; }
    }
}