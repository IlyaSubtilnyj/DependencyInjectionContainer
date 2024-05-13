using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionContainerLib
{
    public interface IDependenciesConfigurationRead
    {
        ImmutableDictionary<Type, Type> Mapper { get; }

        ImmutableDictionary<Type, IEnumerable<Type>> MapperAll { get; }
        ImmutableDictionary<Type, bool> Singletons { get; }

        bool Has(Type dependencyType, out bool is_singleton);

        /// <summary>
        /// Prepare(open generics case) and return implementation type for <paramref name="closedDependencyType"/> dependency.
        /// </summary>
        /// <param name="closedDependencyType"></param>
        /// <returns>Implementation type</returns>
        /// <exception cref="DependenciesConfigurationException">Thrown when an when no implementation is defined for <paramref name="closedDependencyType"/>.</exception>
        Type Get(Type closedDependencyType);

        IEnumerable<Type> GetAll(Type closedDependencyType);
    }
}
