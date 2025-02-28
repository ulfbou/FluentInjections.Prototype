namespace FluentInjections.Abstractions
{
    public interface IAppBuilderAbstraction
    {
        string FrameworkIdentifier { get; } // Using string for FrameworkIdentifier for now
        Type AdapterFactoryType { get; }
        Type ConcreteApplicationType { get; } // Optional but Recommended, so we include it
    }
}
