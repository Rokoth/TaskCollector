using System;
using System.ComponentModel.DataAnnotations;

namespace TaskCollector.Contract.Model
{
    public class Message : Entity
    {
		[Display(Name="Уровень ID")]
		public int Level { get; set; }
		[Display(Name = "Уровень")]
		public string LevelTitle { get; set; }
		[Display(Name = "Заголовок")]
		public string Title { get; set; }
		[Display(Name = "Описание")]
		public string Description { get; set; }
		[Display(Name = "Обратная связь")]
		public string FeedbackContact { get; set; }
		[Display(Name = "Дополнительные поля")]
		public string AddFields { get; set; }
		[Display(Name = "ИД клиента")]
		public Guid ClientId { get; set; }
		[Display(Name = "Дата создания")]
		public DateTimeOffset CreatedDate { get; set; }
		[Display(Name = "Клиент")]
		public string Client { get; set; }
	}

	public class MessageHistory : EntityHistory
	{
		[Display(Name = "Уровень ID")]
		public int Level { get; set; }
		[Display(Name = "Заголовок")]
		public string Title { get; set; }
		[Display(Name = "Описание")]
		public string Description { get; set; }
		[Display(Name = "Обратная связь")]
		public string FeedbackContact { get; set; }
		[Display(Name = "Дополнительные поля")]
		public string AddFileds { get; set; }
		[Display(Name = "ИД клиента")]
		public Guid ClientId { get; set; }
		[Display(Name = "Дата создания")]
		public DateTimeOffset CreatedDate { get; set; }

	}

	public enum MessageLevelEnum
	{ 
	    Issue = 0,
		Warning = 1,
		Error = 10
	}

}
