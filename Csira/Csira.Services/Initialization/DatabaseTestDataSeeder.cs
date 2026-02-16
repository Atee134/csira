using Csira.DataAccess;
using Csira.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Csira.Services.Initialization;

public class DatabaseTestDataSeeder(AppDbContext dbContext)
{
    private static readonly string[] Trees =
    [
        "oak",
        "pine",
        "maple",
        "spruce",
        "cherry",
        "apple",
        "willow",
        "birch",
        "walnut",
        "cedar",
        "elm",
        "rowan",
        "plum",
        "pear",
        "hazel",
        "linden",
        "fir",
        "poplar"
    ];

    private static readonly string[] Plants =
    [
        "tomatoes",
        "roses",
        "lavender",
        "mint",
        "basil",
        "strawberries",
        "cucumbers",
        "zucchini",
        "peppers",
        "thyme",
        "oregano",
        "sage",
        "dill",
        "carrots",
        "lettuce",
        "spinach",
        "beans",
        "sunflowers"
    ];

    private static readonly string[] LawnZones =
    [
        "front lawn",
        "back lawn",
        "side lawn",
        "orchard edge",
        "north strip",
        "south strip",
        "garage side",
        "shed area",
        "fence corner",
        "patio edge",
        "fruit patch border",
        "pond path"
    ];

    private static readonly string[] LeafZones =
    [
        "driveway",
        "patio",
        "garden path",
        "fence line",
        "tool shed entrance",
        "compost corner",
        "mailbox path",
        "garage apron",
        "rain barrel area",
        "pergola walkway",
        "rock garden edge",
        "herb bed border"
    ];

    private static readonly IssuePriority[] Priorities = Enum.GetValues<IssuePriority>();

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        const int targetIssueCount = 300;
        var nowUtc = DateTime.UtcNow;

        var issuesMissingCreatedAt = await dbContext.Issues
            .Where(x => x.CreatedAtUtc == default)
            .ToListAsync(cancellationToken);

        foreach (var issue in issuesMissingCreatedAt)
        {
            issue.CreatedAtUtc = CreateRandomCreatedAtUtc(nowUtc);
        }

        if (issuesMissingCreatedAt.Count > 0)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var existingIssueCount = await dbContext.Issues.CountAsync(cancellationToken);
        if (existingIssueCount >= targetIssueCount)
        {
            return;
        }

        var issuesToCreate = targetIssueCount - existingIssueCount;
        var newIssues = new List<IssueEntity>(issuesToCreate);

        for (var i = 0; i < issuesToCreate; i++)
        {
            newIssues.Add(CreateRandomIssue(nowUtc));
        }

        dbContext.Issues.AddRange(newIssues);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static IssueEntity CreateRandomIssue(DateTime nowUtc)
    {
        var action = Random.Shared.Next(5);
        var createdAtUtc = CreateRandomCreatedAtUtc(nowUtc);

        var (name, description) = action switch
        {
            0 => CreateMowLawnIssue(),
            1 => CreateCutBranchesIssue(),
            2 => CreateRakeLeavesIssue(),
            3 => CreateWaterPlantIssue(),
            _ => CreatePlantTreeIssue()
        };

        return new IssueEntity
        {
            Id = Guid.NewGuid(),
            CreatedAtUtc = createdAtUtc,
            Name = name,
            Description = description,
            Priority = Priorities[Random.Shared.Next(Priorities.Length)]
        };
    }

    private static DateTime CreateRandomCreatedAtUtc(DateTime nowUtc)
    {
        return nowUtc.AddMinutes(-Random.Shared.Next(0, 90 * 24 * 60 + 1));
    }

    private static (string Name, string Description) CreateMowLawnIssue()
    {
        var zone = LawnZones[Random.Shared.Next(LawnZones.Length)];
        return (
            Name: $"Mow the {zone}",
            Description: $"Mow the {zone}, trim the edges, and collect grass clippings for compost."
        );
    }

    private static (string Name, string Description) CreateCutBranchesIssue()
    {
        var tree = Trees[Random.Shared.Next(Trees.Length)];
        return (
            Name: $"Cut branches of the {tree} tree",
            Description: $"Prune dry and overgrown branches from the {tree} tree and dispose of green waste."
        );
    }

    private static (string Name, string Description) CreateRakeLeavesIssue()
    {
        var zone = LeafZones[Random.Shared.Next(LeafZones.Length)];
        return (
            Name: $"Rake leaves near the {zone}",
            Description: $"Rake and bag fallen leaves around the {zone} to keep paths and drains clear."
        );
    }

    private static (string Name, string Description) CreateWaterPlantIssue()
    {
        var plant = Plants[Random.Shared.Next(Plants.Length)];
        return (
            Name: $"Water the {plant}",
            Description: $"Deep-water the {plant} and check soil moisture to prevent overwatering."
        );
    }

    private static (string Name, string Description) CreatePlantTreeIssue()
    {
        var tree = Trees[Random.Shared.Next(Trees.Length)];
        return (
            Name: $"Plant new {tree} trees",
            Description: $"Plant {tree} saplings, add mulch, and secure them with support stakes."
        );
    }
}
