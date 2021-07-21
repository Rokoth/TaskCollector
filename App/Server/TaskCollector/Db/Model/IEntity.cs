//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref2
using System;

namespace TaskCollector.Db.Model
{
    /// <summary>
    /// Common interface for models
    /// </summary>
    public interface IEntity
    {
        Guid Id { get; set; }
        bool IsDeleted { get; set; }
        DateTimeOffset VersionDate { get; set; }
    }
}