//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref2
using System;
using TaskCollector.Db.Attributes;

namespace TaskCollector.Db.Model
{
    /// <summary>
    /// Base DB model class
    /// </summary>
    public abstract class Entity : IEntity
    {
        /// <summary>
        /// Identity
        /// </summary>
        [PrimaryKey]
        [ColumnName("id")]
        public Guid Id { get; set; }
        /// <summary>
        /// Version Date
        /// </summary>
        [ColumnName("version_date")]
        public DateTimeOffset VersionDate { get; set; }
        /// <summary>
        /// Deleted flag
        /// </summary>
        [ColumnName("is_deleted")]
        public bool IsDeleted { get; set; }
    }
}