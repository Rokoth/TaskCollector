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
    public class Entity : IIdentity
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [Display(Name = "Идентификатор")]
        public Guid Id { get; set; }
    }
}