///Copyright 2021 Dmitriy Rokoth
///Licensed under the Apache License, Version 2.0
///
///ref 2
using System;
using System.Collections.Generic;

namespace TaskCollector.Contract.Model
{
    /// <summary>
    /// Filter for client messages
    /// </summary>
    public class MessageFilter : DateFilter<Message>
    {        
        /// <summary>
        /// message title
        /// </summary>
        public string Title { get; }
        /// <summary>
        /// message' client ID
        /// </summary>
        public Guid? ClientId { get; set; }
        /// <summary>
        /// message levels
        /// </summary>
        public List<int> Levels { get; set; }

        /// <summary>
        /// Метод проверки валидности фильтра
        /// </summary>
        protected override bool InternalValid()
        {
            return base.InternalValid();
        }
    }
}