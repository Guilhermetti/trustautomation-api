using Microsoft.EntityFrameworkCore;
using TrustAutomation.Application.Commands.Leads;
using TrustAutomation.Application.Interfaces;
using TrustAutomation.Infrastructure.Data;
using TrustAutomation.Infrastructure.Repositories;
using TrustAutomation.Infrastructure.Security;

namespace TrustAutomation.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTrustAutomation(this IServiceCollection services, IConfiguration cfg)
        {
            var conn = cfg["DATABASE_URL"];
            if (string.IsNullOrWhiteSpace(conn))
                throw new Exception("DATABASE_URL is required.");

            services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(conn));

            services.AddScoped<ILeadRepository, LeadRepository>();
            services.AddSingleton<ISystemClock, SystemClock>();

            services.AddMediatR(cfgM =>
            {
                cfgM.RegisterServicesFromAssemblies(
                    typeof(CreateLeadCommand).Assembly
                );
            });

            return services;
        }
    }
}
