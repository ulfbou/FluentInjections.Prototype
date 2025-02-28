namespace FluentInjections.Abstractions
{
    public interface IApplication<TBuilder>
         where TBuilder : IAppBuilderAbstraction
    {
        Task RunAsync(CancellationToken cancellationToken = default);
        Task StartAsync(CancellationToken cancellationToken = default);
        Task StopAsync(CancellationToken cancellationToken = default);
    }
}