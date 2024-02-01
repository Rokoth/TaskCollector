//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref 2
namespace TaskCollector.Contract.Model
{
    /// <summary>
    /// Filter class
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    public abstract class Filter<T> : IFilter<T> where T : Entity
    {
        /// <summary>
        /// Page size
        /// </summary>
        public int Size { get; set; } = 10;
        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; } = 0;
        /// <summary>
        /// Sort field
        /// </summary>
        public string Sort { get; set; }
    }
}