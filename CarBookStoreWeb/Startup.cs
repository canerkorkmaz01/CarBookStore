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
using System.Security.Claims;
using System.Threading.Tasks;
using CarBookStoreWeb.Sys;
using System.Globalization;

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
            //var cultureInfo = new CultureInfo("tr-TR");
            //CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            //CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

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
            services
            .AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = Configuration.GetValue<bool>("Application:Password:RequireDigit");
                options.Password.RequiredLength = Configuration.GetValue<int>("Application:Password:RequiredLength");
                options.Password.RequireLowercase = Configuration.GetValue<bool>("Application:Password:RequireLowercase");
                options.Password.RequireUppercase = Configuration.GetValue<bool>("Application:Password:RequireUppercase");
                options.Password.RequireNonAlphanumeric = Configuration.GetValue<bool>("Application:Password:RequireNonAlphanumeric");

                options.Lockout.MaxFailedAccessAttempts = Configuration.GetValue<int>("Application:Lockout:MaxFailedAccessAttempts");
                options.Lockout.DefaultLockoutTimeSpan = Configuration.GetValue<TimeSpan>("Application:Lockout:DefaultLockoutTimeSpan");

            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddErrorDescriber<MvcStoreIdentityErrorDescriber>()
            .AddDefaultTokenProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext context, RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "about",
                    pattern: "{controller=Home}/{action=About}/{id?}");

              
            });

            context.Database.Migrate();

            new[]
            {
                new Role { Name = "Administrators", DisplayName = "Yöneticiler" },
                new Role { Name = "Staff", DisplayName = "Personeller" }
            }
           .ToList()
           .ForEach(p =>
           {
               if (!roleManager.RoleExistsAsync(p.Name).Result)
               {
                   roleManager.CreateAsync(p).Wait();
               }
           });

            if (userManager.FindByNameAsync(Configuration.GetValue<string>("Application:DefaultAdmin:UserName")).Result == null)
            {
                var newUser = new User
                {
                    UserName = Configuration.GetValue<string>("Application:DefaultAdmin:UserName"),
                    Name = Configuration.GetValue<string>("Application:DefaultAdmin:Name"),
                    Email = Configuration.GetValue<string>("Application:DefaultAdmin:UserName"),
                    EmailConfirmed = true

                    //SecurityStamp = Guid.NewGuid().ToString()
                };
                //userManager.AddClaimAsync(newUser, new Claim("FullName", newUser.Name)).Wait();


                userManager.CreateAsync(newUser, Configuration.GetValue<string>("Application:DefaultAdmin:Password")).Wait();
                userManager.AddToRoleAsync(newUser, "Administrators").Wait();
            }

        }
    }
}
