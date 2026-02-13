namespace Csira.Services.Initialization;

public interface IApplicationInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken = default);
}
