///Copyright 2021 Dmitriy Rokoth
///Licensed under the Apache License, Version 2.0
///
///ref 1
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using Topshelf;

namespace TaskCollector.TaskCollectorHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            var loggerConfig = new LoggerConfiguration()
               .WriteTo.Console()
               .WriteTo.File("Logs\\log-startup.txt")
               .MinimumLevel.Verbose();

            using var logger = loggerConfig.CreateLogger();
            logger.Information($"Service starts with arguments: {string.Join(", ", args)}");

            var exitCode = HostFactory.Run(x =>
            {
                x.Service<Starter>(s =>
                {
                    s.ConstructUsing(_ => new Starter(logger, args));
                    s.WhenStarted(starter => starter.Start());
                    s.WhenStopped(starter => starter.Stop());
                });

                x.RunAsLocalService();
                x.EnableServiceRecovery(r => r.RestartService(TimeSpan.FromSeconds(10)));
                x.SetDescription($"Task Collector Service, 2021 (�)");
                x.SetDisplayName($"Task Collector Service");
                x.SetServiceName($"Task CollectorService");
                x.StartAutomatically();
            });
            logger.Information($"Service stops with exit code: {exitCode}");
        }
       
    }
}
