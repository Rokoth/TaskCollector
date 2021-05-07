///Copyright 2021 Dmitriy Rokoth
///Licensed under the Apache License, Version 2.0
//////
///ref 2
using System;
using System.ComponentModel.DataAnnotations;

namespace TaskCollector.Contract.Model
{
    /// <summary>
    /// Базовый класс
    /// </summary>
    public class Entity : IEntity
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [Display(Name = "Идентификатор")]
        public Guid Id { get; set; }
    }

    public class EntityHistory : Entity
    {
        public long HId { get; set; }
        public DateTimeOffset ChangeDate { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class ErrorMessage
    { 
        public string Message { get; set; }
        public string Source { get; set; }
    }
}