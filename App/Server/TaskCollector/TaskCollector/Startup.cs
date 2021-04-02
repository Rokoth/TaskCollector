using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskCollector
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    public static class CustomExtensionMethods
    {
        public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));

            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<TreeCreator, Tree>()
            //    .ForMember(s => s.Id, s => s.MapFrom(c => Helper.GenerateGuid(new string[] { c.Name })))
            //    .ForMember(s => s.VersionDate, s => s.MapFrom(c => DateTimeOffset.Now));

            //CreateMap<TreeUpdater, Tree>()
            //    .ForMember(s => s.Id, s => s.MapFrom(c => Helper.GenerateGuid(new string[] { c.Name })))
            //    .ForMember(s => s.VersionDate, s => s.MapFrom(c => DateTimeOffset.Now));

            //CreateMap<Tree, TreeModel>();

            //CreateMap<TreeItem, TreeItemModel>();

            //CreateMap<FormulaCreator, Formula>();

            //CreateMap<FormulaUpdater, Formula>();

            //CreateMap<Formula, FormulaModel>();
            //CreateMap<TreeHistory, TreeHistoryModel>();
            //CreateMap<TreeItemHistory, TreeItemHistoryModel>();
            //CreateMap<FormulaHistory, FormulaHistoryModel>();
        }
    }
}
