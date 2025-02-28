namespace FluentInjections.Abstractions.Adapters
{
    public interface IConcreteApplicationAdapter
    {
        Task StartAsync(CancellationToken cancellationToken = default);
        Task StopAsync(CancellationToken cancellationToken = default);
        Task RunAsync(CancellationToken cancellationToken = default);
    }
}