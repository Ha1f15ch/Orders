using ApplicationDbContext;
using ApplicationDbContext.ContextRepositories;
using ApplicationDbContext.ContextRepositories.Services;
using ApplicationDbContext.Interfaces;
using ApplicationDbContext.Interfaces.ServicesInterfaces;
using ApplicationDbContext.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ModelsEntity;
using SiteEngine.Controllers;
using SiteEngine.Controllers.ChatComponents;
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
            builder.Services.AddTransient<IServiceInterfaceGetCookieData, ServiceGetCookieData>();
            builder.Services.AddTransient<IOrderPerformerMappingRepositories, OrderPerformerMappingRepositories>();
            builder.Services.AddTransient<IQueueOrderCancellationsRepositories, QueueOrderCancellationsRepositories>();
            builder.Services.AddTransient<IOrderScoreRepositories, OrderScoreRepositories>();
            builder.Services.AddTransient<IChatRepositories, ChatRepositories>();
            builder.Services.AddTransient<IMessageRepositories, MessageRepositories>();
            builder.Services.AddTransient<IRedisChatService, RedisChatService>();
            builder.Services.AddTransient<IChatService, ChatService>();
            builder.Services.AddTransient<ChatHub>();
            builder.Services.AddSignalR();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSignalR();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options => options.LoginPath = "/account");
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("Home/Error");
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
            app.UseCors("AllowAllOrigins");
            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHub<ChatHub>("/chatHub");


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

            app.MapControllerRoute(
                name: "CustomerChat",
                pattern: "{controller=CustomerChat}/{action=Index}/{id?}"
            );

            app.MapControllerRoute(
                name: "PerformerChat",
                pattern: "{controller=PerformerChat}/{action=Index}/{id?}"
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
