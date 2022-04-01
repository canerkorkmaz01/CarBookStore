using CarBookData;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarBookStoreWeb
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
            services.AddRazorPages();
            services.AddControllersWithViews()

            .AddNewtonsoftJson(options =>
             {
                 options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
             });

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                var dataBase = Configuration.GetValue<string>("Application:Database");
                switch (dataBase)
                {
                    case "mysql":
                        options.UseMySql(
                            Configuration.GetConnectionString("MySql"),
                            ServerVersion.AutoDetect(Configuration.GetConnectionString("Mysql")),
                            config =>
                            {
                                config.MigrationsAssembly("MigrationMySql");
                            }
                            );
                        break;
                    case "sqlserver":
                    default:
                        options.UseSqlServer(
                            Configuration.GetConnectionString("SqlServer"),
                            config =>
                            {
                                config.MigrationsAssembly("MigrationSqlServer");
                            });
                        break;
                }

            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext context/*, RoleManager<Role> roleManager, UserManager<User> userManager*/)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "about",
                    pattern: "{controller=Home}/{action=About}/{id?}");

                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
            });

            context.Database.Migrate();
        }
    }
}
