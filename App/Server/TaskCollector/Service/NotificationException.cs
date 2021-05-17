﻿using System;
using System.Runtime.Serialization;

namespace TaskCollector.Service
{
    [Serializable]
    internal class NotificationException : Exception
    {
        public NotificationException()
        {
        }

        public NotificationException(string message) : base(message)
        {
        }

        public NotificationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotificationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}