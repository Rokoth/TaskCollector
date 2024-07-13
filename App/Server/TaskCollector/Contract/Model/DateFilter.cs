///Copyright 2021 Dmitriy Rokoth
///Licensed under the Apache License, Version 2.0
///
///ref 1
using System;

namespace TaskCollector.Contract.Model
{
    /// <summary>
    /// Filter for client messages
    /// </summary>
    public abstract class DateFilter<T> : Filter<T> where T : Entity
    {       
        /// <summary>
        /// message create date from
        /// </summary>
        public DateTimeOffset? DateFrom { get; set; }
        /// <summary>
        /// message create date to
        /// </summary>
        public DateTimeOffset? DateTo { get; set; }
    }
}