//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref2
namespace TaskCollector.Contract.Model
{
    /// <summary>
    /// Обобщенный интерфейс классов фильтра
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFilter<T> where T : Entity
    {
        /// <summary>
        /// Страница
        /// </summary>
        int Page { get; }
        /// <summary>
        /// Размер
        /// </summary>
        int Size { get; }
        /// <summary>
        /// Поле сортировки
        /// </summary>
        string Sort { get; }
    }
}