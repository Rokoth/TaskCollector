///Copyright 2021 Dmitriy Rokoth
///Licensed under the Apache License, Version 2.0
///
///ref 1
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using TaskCollector.Deploy;

namespace TaskCollector.TaskCollectorHost
{
    public static class CustomExtensionMethods
    {
        public static IConfigurationBuilder AddDbConfiguration(this IConfigurationBuilder builder, ILogger logger)
        {
            try
            {
                var configuration = builder.Build();
                var connectionString = configuration.GetConnectionString("MainConnection");
                builder.Add(new ConfigDbSource(options => options.UseNpgsql(connectionString)));
                return builder;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка при добавлении конфигурации из БД");
                throw;
            }
        }

        public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
        {
            var mappingConfig = new AutoMapper.MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));

            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }

        public static IWebHostBuilder DeployDataBase(this IWebHostBuilder builder, IConfigurationBuilder configurationBuilder, ILogger logger)
        {
            try
            {
                var config = configurationBuilder.Build();
                var connectionString = config.GetConnectionString("MainConnection");
                var deployService = new DeployService(connectionString);
                deployService.Deploy().ConfigureAwait(false).GetAwaiter().GetResult();

                return builder;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка при обновлении БД");
                throw;
            }
        }
    }
}
