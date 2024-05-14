using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace DependencyInjectionContainerLib
{
    public interface IDependenciesConfigurationWrite: IDependenciesConfigurationRead
    {
        /**
         * <summary>
         * Instance per dependency
         * Each new dependency request from container leads to creation of new TImplementation object
         * </summary>
         */
        void Register<TDependency, TImplementation>()
            where TDependency : class
            where TImplementation : TDependency;


        /**
        * <summary>
         * Singleton
         * On each dependency request the only one object returned
         * </summary>
         */
        void Singleton<TDependency, TImplementation>()
            where TDependency : class
            where TImplementation : TDependency;

        /**
         * <summary>
         * Open generics
         * Instance per dependency
         * Each new dependency request from container leads to creation of new TImplementation object
         * </summary>
         * <exception cref="DependenciesConfigurationException">Thrown when an error occurs.</exception>
         */
        void Register(Type OpenDependencyGeneric, Type OpenImplementationGeneri);

        /**
         * <summary>
         * Open generics
         * Singleton
         * On each dependency request the only one object returned
         * </summary>
         * <exception cref="DependenciesConfigurationException">Thrown when an error occurs.</exception>
         */
        void Singleton(Type OpenDependencyGeneric, Type OpenImplementationGeneri);
    }
}
