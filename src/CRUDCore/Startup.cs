using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using CRUDCore.Data;

namespace CRUDCore
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<TaskManagementDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), x => x.UseRowNumberForPaging()));
            
            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.CookieName = ".MyApplication";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, TaskManagementDbContext context)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error/Index");
            }

            app.UseStaticFiles();
            app.UseSession();
            
            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=CategoryTasks}/{action=Index}/{id?}");

                routes.MapRoute(
                   name: "Category Tasks",
                   template: "cates",
                  defaults: new { controller = "CategoryTasks", action = "Index" });

                routes.MapRoute(
                   name: "Tasks",
                   template: "tasks",
                  defaults: new { controller = "Tasks", action = "Index" });

                routes.MapRoute(
                   name: "Manager Tasks ",
                   template: "tasks/manager/{id}",
                  defaults: new { controller = "Tasks", action = "Manager" });

                routes.MapRoute(
                   name: "Manager Category Tasks",
                   template: "cates/manager/{id}",
                  defaults: new { controller = "CategoryTasks", action = "Manager" });
                
            });
            DbInitializer.Initialize(context);
        }
    }
}
