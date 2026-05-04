using MeetIQ.Application.DependencyInjection;
using MeetIQ.Domain.Entities;
using MeetIQ.Infrastructure.DependencyInjection;
using MeetIQ.Infrastructure.Identity;
using MeetIQ.Web.Filters;
using Microsoft.AspNetCore.Identity;

namespace MeetIQ.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            //builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllersWithViews();
            //builder.Services.AddControllersWithViews(options =>
            //{
            //    options.Filters.Add<GlobalExceptionFilter>();
            //});

            //  Cookie config 
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
                options.LogoutPath = "/Auth/Logout";
                options.AccessDeniedPath = "/Auth/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.SlidingExpiration = true;
            });

            builder.Services.AddAuthentication()
                .AddGoogle(options =>
                {
                    var google = builder.Configuration.GetSection("Authentication:Google");

                    options.ClientId = google["ClientId"]!;
                    options.ClientSecret = google["ClientSecret"]!;
                    options.CallbackPath = "/signin-google";

                    options.Scope.Add("profile");
                });

            var app = builder.Build();


            //Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.Use(async (context, next) =>
            //{
            //    try{ await next();}
            //    catch{context.Response.Redirect("/Home/Error");}
            //});

            using (var scope = app.Services.CreateScope())
            {
                await scope.ServiceProvider.SeedIdentityAsync();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
