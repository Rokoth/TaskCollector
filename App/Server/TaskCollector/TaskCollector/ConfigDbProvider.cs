///Copyright 2021 Dmitriy Rokoth
///Licensed under the Apache License, Version 2.0
///
///ref 2
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using TaskCollector.Db.Context;

namespace TaskCollector.TaskCollectorHost
{
    /// <summary>
    /// Получение конфигурации из БД
    /// </summary>
    public class ConfigDbProvider : ConfigurationProvider
    {
        private readonly DbContextOptions<DbPgContext> _options;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="optionsAction"></param>
        public ConfigDbProvider(Action<DbContextOptionsBuilder> optionsAction)
        {
            var builder = new DbContextOptionsBuilder<DbPgContext>();
            optionsAction(builder);
            _options = builder.Options;
        }

        /// <summary>
        /// Загрузка конфигурации
        /// </summary>
        public override void Load()
        {
            using var context = new DbPgContext(_options);
            var items = context.Settings.AsNoTracking().ToList();
            foreach (var item in items.Where(s => !Data.ContainsKey(s.ParamName)))
                Data.Add(item.ParamName, item.ParamValue);
        }
    }
}
