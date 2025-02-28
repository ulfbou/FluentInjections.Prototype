using Microsoft.Extensions.Logging;

namespace FluentInjections.Abstractions.Factories
{
    public interface IBuilderFactory<TBuilder>
        where TBuilder : IAppBuilderAbstraction
    {
        TBuilder CreateBuilder(string[]? args = null, ILoggerFactory? loggerFactory = null /*, IConfigurationSourceProvider? configurationSourceProvider = null - Let's hold off on config for now, simplify initial interface */);
    }
}
