///Copyright 2021 Dmitriy Rokoth
///Licensed under the Apache License, Version 2.0
//////
///ref 1
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskCollector.Common;
using TaskCollector.Db.Context;
using TaskCollector.Db.Interface;
using TaskCollector.Db.Repository;
using TaskCollector.Deploy;
using TaskCollector.Service;

namespace TaskCollector.TaskCollectorHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {            
            services.Configure<CommonOptions>(Configuration);
            services.Configure<NotifyOptions>(Configuration.GetSection("NotifyOptions"));
            services.AddControllersWithViews();
            services.AddDbContextPool<DbPgContext>((opt) =>
            {
                opt.EnableSensitiveDataLogging();
                var connectionString = Configuration.GetConnectionString("MainConnection");
                opt.UseNpgsql(connectionString);
            });

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options=> {
            //        options.LoginPath = new PathString("/Account/Login");                    
            //    });
            services.AddCors();
            services.AddLogging();
            services.AddAuthentication()           
            .AddJwtBearer("Token", options =>
            {
                options.RequireHttpsMetadata = false;                
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //// укзывает, будет ли валидироваться издатель при валидации токена
                    ValidateIssuer = true,
                    //// строка, представляющая издателя
                    ValidIssuer = AuthOptions.ISSUER,

                    //// будет ли валидироваться потребитель токена
                    ValidateAudience = true,
                    //// установка потребителя токена
                    ValidAudience = AuthOptions.AUDIENCE,
                    //// будет ли валидироваться время существования
                    ValidateLifetime = true,                    

                    // установка ключа безопасности
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    // валидация ключа безопасности
                    ValidateIssuerSigningKey = true,
                    
                };
            }).AddCookie("Cookies", options=> {
                options.LoginPath = new PathString("/Account/Login");                
            });

            services
                .AddAuthorization(options =>
                {
                    var cookiePolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .AddAuthenticationSchemes("Cookies")
                        .Build();
                    options.DefaultPolicy = cookiePolicy;
                    options.AddPolicy("Cookie", cookiePolicy);
                    options.AddPolicy("Token", new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .AddAuthenticationSchemes("Token")
                        .Build());                   
                });

            services.AddScoped<IRepository<Db.Model.User>, Repository<Db.Model.User>>();
            services.AddScoped<IRepository<Db.Model.Client>, Repository<Db.Model.Client>>();
            services.AddScoped<IRepository<Db.Model.Message>, Repository<Db.Model.Message>>();
            services.AddScoped<IRepository<Db.Model.MessageStatus>, Repository<Db.Model.MessageStatus>>();
            services.AddScoped<IRepository<Db.Model.UserHistory>, Repository<Db.Model.UserHistory>>();
            services.AddScoped<IRepository<Db.Model.ClientHistory>, Repository<Db.Model.ClientHistory>>();
            services.AddScoped<IRepository<Db.Model.MessageHistory>, Repository<Db.Model.MessageHistory>>();
            services.AddScoped<IRepository<Db.Model.MessageStatusHistory>, Repository<Db.Model.MessageStatusHistory>>();
            services.AddDataServices();
            services.AddScoped<IDeployService, DeployService>();
            services.AddScoped<INotifyService, NotifyService>();
            services.ConfigureAutoMapper();
            services.AddSwaggerGen(swagger =>
            {
                //s.OperationFilter<AddRequiredHeaderParameter>();

                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
            });
            services.AddHostedService<NotificationCheckTaskHostedService>();
        }
                
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthentication();
            
            //var cookiePolicyOptions = new CookiePolicyOptions
            //{
            //    MinimumSameSitePolicy = SameSiteMode.Strict,                
            //};
            //app.UseCookiePolicy(cookiePolicyOptions);
           
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Description = "access token",
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new OpenApiString("Bearer ")
                }
            });
        }
    }
}
