//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref2
using System;
using System.Runtime.Serialization;

namespace TaskCollector.Service
{
    /// <summary>
    /// Исключение, сгенерированное в DataService
    /// </summary>
    [Serializable]
    internal class DataServiceException : Exception
    {
        /// <summary>
        /// ctor
        /// </summary>
        public DataServiceException()
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="message"></param>
        public DataServiceException(string message) : base(message)
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public DataServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected DataServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}