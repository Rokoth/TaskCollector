using System;
using System.ComponentModel.DataAnnotations;

namespace TaskCollector.Contract.Model
{
    public enum MessageStatusEnum
    { 
        New = 0,
        Working = 1,
        Deferred = 2,
        Ready = 3,
        Closed = 100
    }

    public class MessageStatus : Entity
    {
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
        public string Message { get; set; }
        [Display(Name = "Пользователь")]
        public User User { get; set; }
    }       

    public class MessageStatusHistory : EntityHistory
    {
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
    }

}
