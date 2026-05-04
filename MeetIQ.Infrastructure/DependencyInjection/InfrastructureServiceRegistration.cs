using MeetIQ.Application.Interfaces;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Application.Interfaces.Services;
using MeetIQ.Application.Services;
using MeetIQ.Domain.Entities;
using MeetIQ.Infrastructure.Persistence.Repositories;
using MeetIQ.Infrastructure.Presistence;
using MeetIQ.Infrastructure.Presistence.Repositories;
using MeetIQ.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlKata.Compilers;
using SqlKata.Execution;
using System.Data;

namespace MeetIQ.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(
           this IServiceCollection services,
           IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            // IDbConnection - Dapper
            services.AddScoped<IDbConnection>(sp =>
                            new SqlConnection(configuration.GetConnectionString("DefaultConnection")));

            // SqlKata
            services.AddScoped<QueryFactory>(sp =>
            {
                var connection = sp.GetRequiredService<IDbConnection>();

                var compiler = new SqlServerCompiler();

                return new QueryFactory(connection, compiler);
            });

            // Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IMeetingRepository, MeetingRepository>();
            services.AddScoped<IMeetingParticipantRepository, MeetingParticipantRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<INoteTagRepository, NoteTagRepository>();
            services.AddScoped<ICalendarEventRepository, CalendarEventRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            // Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddSingleton<JitsiTokenService>();
            services.AddScoped<INotificationService, NotificationService>();

            return services;
        }
    }
}
