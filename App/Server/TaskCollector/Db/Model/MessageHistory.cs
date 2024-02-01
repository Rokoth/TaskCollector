//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref 2
using System;
using TaskCollector.Db.Attributes;

namespace TaskCollector.Db.Model
{
    /// <summary>
    /// Модель истории сообщения
    /// </summary>
    [TableName("h_message")]
    public class MessageHistory : EntityHistory
    {
        /// <summary>
        /// Уровень ошибки
        /// </summary>
        [ColumnName("level")]
        public int Level { get; set; }

        /// <summary>
        /// Заголовок
        /// </summary>
        [ColumnName("title")]
        public string Title { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        [ColumnName("description")]
        public string Description { get; set; }

        /// <summary>
        /// Обратная связь
        /// </summary>
        [ColumnName("feedback_contact")]
        public string FeedbackContact { get; set; }

        /// <summary>
        /// ИД клиента
        /// </summary>
        [ColumnName("client_id")]
        public Guid ClientId { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        [ColumnName("created_date")]
        public DateTimeOffset CreatedDate { get; set; }

        /// <summary>
        /// Дополнительные поля
        /// </summary>
        [ColumnName("add_fields")]
        [ColumnType("json")]
        public string AddFields { get; set; }
    }
}
