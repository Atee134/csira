using Csira.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Csira.Services.Initialization;

public class DatabaseInitializer(AppDbContext dbContext) : IApplicationInitializer
{
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.Database.MigrateAsync(cancellationToken);
    }
}
