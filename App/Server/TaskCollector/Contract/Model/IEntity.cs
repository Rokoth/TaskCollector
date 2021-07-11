//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref 2
using System;

namespace TaskCollector.Contract.Model
{
    /// <summary>
    /// interface: Guid Id field
    /// </summary>
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}