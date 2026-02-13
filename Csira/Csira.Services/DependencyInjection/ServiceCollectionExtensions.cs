using Csira.DataAccess;
using Csira.Services.Infrastructure;
using Csira.Services.Issues;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Csira.Services.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
        services.AddScoped<IIssueService, IssueService>();
        services.AddHostedService<DatabaseInitializationHostedService>();

        return services;
    }
}