//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref 2
using System;
using System.ComponentModel.DataAnnotations;

namespace TaskCollector.Contract.Model
{
    /// <summary>
	/// Сообщение
	/// </summary>
	public class Message : Entity
    {
		/// <summary>
		/// Уровень ID
		/// </summary>
		[Display(Name="Уровень ID")]
		public int Level { get; set; }
		/// <summary>
		/// Уровень
		/// </summary>
		[Display(Name = "Уровень")]
		public string LevelTitle { get; set; }
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
		public string AddFields { get; set; }
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
		/// <summary>
		/// Клиент
		/// </summary>
		[Display(Name = "Клиент")]
		public string Client { get; set; }
	}
}
