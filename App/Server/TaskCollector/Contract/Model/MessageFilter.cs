///Copyright 2021 Dmitriy Rokoth
///Licensed under the Apache License, Version 2.0
///
///ref 1
using System;
using System.Collections.Generic;

namespace TaskCollector.Contract.Model
{
    /// <summary>
    /// Filter for client messages
    /// </summary>
    public class MessageFilter : Filter<Message>
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="size"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="title"></param>
        /// <param name="clientId"></param>
        /// <param name="levels"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        public MessageFilter(int size, int page, string sort, string title, Guid? clientId,
            List<int> levels, DateTimeOffset? dateFrom, DateTimeOffset? dateTo) : base()
        {
            Title = title;
            ClientId = clientId;
            Levels = levels;
            DateFrom = dateFrom;
            DateTo = dateTo;
            Sort = sort;
            Size = size;
            Page = page;
        }
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
        /// message create date from
        /// </summary>
        public DateTimeOffset? DateFrom { get; set; }        
        /// <summary>
        /// message create date to
        /// </summary>
        public DateTimeOffset? DateTo { get; set; }
    }
}