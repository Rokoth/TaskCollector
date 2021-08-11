//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref2
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using TaskCollector.Db.Model;

namespace TaskCollector.Db.Context
{
    /// <summary>
    /// Db context for postgres db
    /// </summary>
    public class DbPgContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageStatus> MessageStatuses { get; set; }
        public DbSet<Settings> Settings { get; set; }

        public DbPgContext(DbContextOptions<DbPgContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("uuid-ossp");
           
            modelBuilder.ApplyConfiguration(new EntityConfiguration<Settings>());

            foreach (var type in Assembly.GetAssembly(typeof(Entity)).GetTypes())
            {
                if (typeof(IEntity).IsAssignableFrom(type) && !type.IsAbstract)
                {                   
                    var config = Activator.CreateInstance(typeof(EntityConfiguration<>).MakeGenericType(type));                    
                    GetType().GetMethod(nameof(ApplyConf), BindingFlags.NonPublic | BindingFlags.Instance)
                        .MakeGenericMethod(type).Invoke(this, new object[] { modelBuilder, config });
                }
            }
        }

        private void ApplyConf<T>(ModelBuilder modelBuilder, EntityConfiguration<T> config) where T : class, IEntity 
        {
            modelBuilder.ApplyConfiguration(config);
        }
    }
}
