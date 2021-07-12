//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref2
namespace TaskCollector.Contract.Model
{
    /// <summary>
    /// interface of identity classes
    /// </summary>
    public interface IIdentity
    {
        string Login { get; set; }
        string Password { get; set; }
    }
}