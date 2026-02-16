using Csira.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Csira.Services.Initialization;

public class DatabaseInitializer(
    AppDbContext dbContext,
    IConfiguration configuration,
    DatabaseTestDataSeeder databaseTestDataSeeder) : IApplicationInitializer
{
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.Database.MigrateAsync(cancellationToken);

        var shouldSeedTestData = bool.TryParse(
            configuration["Development:SeedTestData"],
            out var parsedSeedTestData)
            && parsedSeedTestData;

        if (shouldSeedTestData)
        {
            await databaseTestDataSeeder.SeedAsync(cancellationToken);
        }
    }
}
