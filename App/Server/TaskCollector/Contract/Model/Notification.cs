///Copyright 2021 Dmitriy Rokoth
///Licensed under the Apache License, Version 2.0
//////
///ref 2
using System;
using System.Collections.Generic;

namespace TaskCollector.Contract.Model
{
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