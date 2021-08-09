//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref2
using System.Threading.Tasks;

namespace TaskCollector.Deploy
{
    /// <summary>
    /// Deploy Service interface
    /// </summary>
    public interface IDeployService
    {
        /// <summary>
        /// Deploy DB method
        /// </summary>
        /// <param name="num">last update</param>
        /// <returns></returns>
        Task Deploy(int? num = null);
    }
}