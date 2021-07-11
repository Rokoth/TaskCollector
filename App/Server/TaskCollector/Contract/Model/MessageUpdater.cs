using System;
using System.ComponentModel.DataAnnotations;

namespace TaskCollector.Contract.Model
{
    public class MessageUpdater : IEntity
    {
        public Guid Id { get; set; }
		[Display(Name = "Уровень ID")]
		public int Level { get; set; }
		[Display(Name = "Заголовок")]
		public string Title { get; set; }
		[Display(Name = "Описание")]
		public string Description { get; set; }			
	}

    public class MessageStatusUpdater : IEntity
    {
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        [Display(Name = "ID сообщения")]
        public Guid MessageId { get; set; }
        [Display(Name = "ID статуса")]
        public MessageStatusEnum StatusId { get; set; }
        [Display(Name = "Статус")]
        public string Status => Enum.GetName(typeof(MessageStatusEnum), StatusId);
        [Display(Name = "ID пользователя")]
        public Guid UserId { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Текущий статус")]
        public bool IsLast { get; set; }
        [Display(Name = "Дата статуса")]
        public DateTimeOffset StatusDate { get; set; }
        [Display(Name = "Дата следующего уведомления")]
        public DateTimeOffset? NextNotifyDate { get; set; }
        [Display(Name = "Сообщение")]
        public Message Message { get; set; }
        [Display(Name = "Пользователь")]
        public User User { get; set; }
    }

    public class MessageStatusCreator
    {

    }

}
