namespace FluentInjections.Abstractions
{
    /// <summary>
    /// Represents metadata for an application type.
    /// </summary>
    public interface IApplicationTypeMetadata
    {
        /// <summary>
        /// Gets the type of the application type.
        /// </summary>
        Type ConcreteType { get; }

        /// <summary>
        /// Gets the type of the adapter that is used to build the application type.
        /// </summary>
        Type AdapterType { get; }

        /// <summary>
        /// Gets the name of the assembly that contains the application type.
        /// </summary>
        string? AssemblyName { get; }

        /// <summary>
        /// Gets the name of the application type.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the name of the framework that the application type targets.
        /// </summary>
        string FrameworkName { get; }

        /// <summary>
        /// Gets the version of the framework that the application type targets.
        /// </summary>
        string FrameworkVersion { get; }
    }
}
