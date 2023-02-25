//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref2
using System;
using System.ComponentModel.DataAnnotations;

namespace TaskCollector.Contract.Model
{
    public class MessageHistory : EntityHistory
	{
		/// <summary>
		/// Уровень сообщения
		/// </summary>
		[Display(Name = "Уровень")]
		public int Level { get; set; }

        /// <summary>
        /// Заголовок
        /// </summary>
        [Display(Name = "Заголовок")]
		public string Title { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        [Display(Name = "Описание")]
		public string Description { get; set; }

        /// <summary>
        /// Обратная связь
        /// </summary>
        [Display(Name = "Обратная связь")]
		public string FeedbackContact { get; set; }

        /// <summary>
        /// Дополнительные поля
        /// </summary>
        [Display(Name = "Дополнительные поля")]
		public string AddFileds { get; set; }

        /// <summary>
		/// ИД клиента
		/// </summary>
		[Display(Name = "ИД клиента")]
		public Guid ClientId { get; set; }

        /// <summary>
		/// Дата создания
		/// </summary>
		[Display(Name = "Дата создания")]
		public DateTimeOffset CreatedDate { get; set; }
	}
}
