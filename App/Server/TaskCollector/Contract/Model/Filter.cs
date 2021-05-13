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
        /// ctor
        /// </summary>
        /// <param name="size">Page size</param>
        /// <param name="page">Page number</param>
        /// <param name="sort">Sort field</param>
        public Filter(int size, int page, string sort)
        {
            Size = size;
            Page = page;
            Sort = sort;
        }
        /// <summary>
        /// Page size
        /// </summary>
        public int Size { get; }
        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; }
        /// <summary>
        /// Sort field
        /// </summary>
        public string Sort { get; }
    }
}