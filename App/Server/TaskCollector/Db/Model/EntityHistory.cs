//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref2
using System;
using TaskCollector.Db.Attributes;

namespace TaskCollector.Db.Model
{
    /// <summary>
    /// history table common model
    /// </summary>
    public abstract class EntityHistory : IEntity
    {
        /// <summary>
        /// history id
        /// </summary>
        [PrimaryKey]
        [ColumnName("h_id")]
        public long HId { get; set; }
        /// <summary>
        /// change date
        /// </summary>
        [ColumnName("change_date")]
        public DateTimeOffset ChangeDate { get; set; }
        /// <summary>
        /// main model id
        /// </summary>
        [ColumnName("id")]
        public Guid Id { get; set; }
        /// <summary>
        /// main model version date
        /// </summary>
        [ColumnName("version_date")]
        public DateTimeOffset VersionDate { get; set; }
        /// <summary>
        /// main model is deleted
        /// </summary>
        [ColumnName("is_deleted")]
        public bool IsDeleted { get; set; }
    }
}