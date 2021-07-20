using System;
using System.ComponentModel.DataAnnotations;

namespace TaskCollector.Contract.Model
{
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

}
