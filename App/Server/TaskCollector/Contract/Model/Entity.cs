///Copyright 2021 Dmitriy Rokoth
///Licensed under the Apache License, Version 2.0
//////
///ref 2
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskCollector.Contract.Model
{
    /// <summary>
    /// Базовый класс
    /// </summary>
    public class Entity : IEntity
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [Display(Name = "Идентификатор")]
        public Guid Id { get; set; }
    }

    public class EntityHistory : Entity
    {
        public long HId { get; set; }
        public DateTimeOffset ChangeDate { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class ErrorMessage
    { 
        public string Message { get; set; }
        public string Source { get; set; }
    }

    public enum NotificationTypeEnum
    { 
        MessageReceived = 1,
        TakeInWorkTimeout = 2,
        WorkTimeout = 3,
        Expired = 4
    }

    public class Notification
    {
        private Dictionary<NotificationTypeEnum, string> _notificationTypeDictionary = new Dictionary<NotificationTypeEnum, string>()
        {
            { NotificationTypeEnum.MessageReceived, "Сообщение получено"},
            { NotificationTypeEnum.TakeInWorkTimeout, "Таймаут взятия в работу"},
            { NotificationTypeEnum.WorkTimeout, "Таймаут обработки"},
            { NotificationTypeEnum.Expired, "Сообщение просрочено"}
        };

        public NotificationTypeEnum NotificationTypeEnum { get; set; }

        public string NotificationType => _notificationTypeDictionary[NotificationTypeEnum];
        public string Level { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FeedbackContact { get; set; }
        public string AddFields { get; set; }
        public string Client { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }

}