using Hangfire;
using MeetIQ.Application.Common.Options;
using MeetIQ.Application.DependencyInjection;
using MeetIQ.Application.Interfaces.Services;
using MeetIQ.Infrastructure.DependencyInjection;
using MeetIQ.Infrastructure.Identity;
using MeetIQ.Infrastructure.Jobs;
using MeetIQ.Infrastructure.Services;
using MeetIQ.Infrastructure.SignalR;
using MeetIQ.Web.Infrastructure;

namespace MeetIQ.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Services 
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddControllersWithViews();


            //builder.Services.AddControllersWithViews(options =>
            //            //{
            //            //    options.Filters.Add<GlobalExceptionFilter>();
            //            //});

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

            // Kestrel Configuration
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 500 * 1024 * 1024; // 500 MB
            });

           
            builder.Services.AddScoped<TaskDueSoonJob>();
            builder.Services.AddScoped<TaskOverdueJob>();
            builder.Services.AddScoped<MeetingStartingSoonJob>();

            // Build 
            var app = builder.Build();

            // Pipeline 
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //app.Use(async (context, next) =>
            //{
            //    try{ await next();}
            //    catch{context.Response.Redirect("/Home/Error");}
            //});

            // Seed 
            using (var scope = app.Services.CreateScope())
            {
                await scope.ServiceProvider.SeedIdentityAsync();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // Hangfire Dashboard
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = [new HangfireAdminAuthFilter()]
            });


            // Recurring Jobs 
            using (var scope = app.Services.CreateScope())
            {
                var recurringJobManager = scope.ServiceProvider
                    .GetRequiredService<IRecurringJobManager>();

                recurringJobManager.AddOrUpdate<TaskDueSoonJob>(
                    recurringJobId: "task-due-soon",
                    methodCall: job => job.RunAsync(),
                    cronExpression: Cron.Hourly()
                );

                recurringJobManager.AddOrUpdate<TaskOverdueJob>(
                    recurringJobId: "task-overdue",
                    methodCall: job => job.RunAsync(),
                    cronExpression: "0 9 * * *"
                );

                recurringJobManager.AddOrUpdate<MeetingStartingSoonJob>(
                    recurringJobId: "meeting-starting-soon",
                    methodCall: job => job.RunAsync(),
                    cronExpression: "*/15 * * * *"
                );
            }

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapHub<NotificationHub>("/hubs/notifications");


            app.Run();
        }
    }
}