using ApplicationDbContext;
using ApplicationDbContext.ContextRepositories;
using ApplicationDbContext.Interfaces;
using ApplicationDbContext.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ModelsEntity;
using SiteEngine.Controllers;
using SiteEngine.Helpers;

namespace SiteEngine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .Build();

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton<IConfiguration>(config);
            AddDatabaseContext(builder.Services, config);

            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            builder.Services.AddTransient<IServiceRepository, ServiceRepository>();
            builder.Services.AddTransient<IProfileCustomerRepositories, ProfileCustomerRepositories>();
            builder.Services.AddTransient<IProfilePerformerRepositories, ProfilePerformerRepositories>();
            builder.Services.AddTransient<IPerformerServiceMappingRepositories, PerformerServiceMappingRepositories>();
            builder.Services.AddTransient<IOrderRepositories, OrderRepositories>();
            builder.Services.AddTransient<IOrderPriorityRepositories, OrderPriorityRepositories>();
            builder.Services.AddTransient<IOrderStatusRepositories, OrderStatusRepositories>();

            /*builder.Services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new CustomViewLocationExpander());
            });*/

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options => options.LoginPath = "/account");
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/NotFound";
                    await next();
                }
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "/",
                pattern: "{controller=MainPage}/{action=Index}"
            );
            app.MapControllerRoute(
                name: "/Home",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );
            app.MapControllerRoute(
                name: "/account",
                pattern: "{controller=Account}/{action=Index}"
            );
            app.MapControllerRoute(
                name: "/amplua",
                pattern: "{controller=Amplua}/{action=Index}"
            );
            app.MapControllerRoute(
                name: "CustomerBoard",
                pattern: "{controller=CustomerBoard}/{action=Index}/{id?}"
            );
            app.MapControllerRoute(
                name: "PerformerBoard",
                pattern: "{controller=PerformerBoard}/{action=Index}"
            );

            app.Map("/users/list", () => "LIST with Users");
            

            app.Run();
        }

        private static void AddDatabaseContext(IServiceCollection services, IConfiguration config)
        {
            var excelDataFilePath = config.GetConnectionString("ExcelDataPath");

            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(config.GetConnectionString("DefaultDatabaseConnection"))
                .Options;

            var appDbContext = new AppDbContext(dbContextOptions, excelDataFilePath);
            services.AddSingleton<AppDbContext>(appDbContext);
        }

    }
}
