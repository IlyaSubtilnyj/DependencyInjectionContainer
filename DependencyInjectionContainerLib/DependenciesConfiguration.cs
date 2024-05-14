using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionContainerLib
{
    public class DependenciesConfiguration : IDependenciesConfigurationWrite
    {

        private ConcurrentDictionary<Type, Type> _mapper;
        private ConcurrentDictionary<Type, IEnumerable<Type>> _mapper_all;
        private ConcurrentDictionary<Type, bool> _singleton;

        public ImmutableDictionary<Type, Type> Mapper => _mapper.ToImmutableDictionary();
        public ImmutableDictionary<Type, IEnumerable<Type>> MapperAll => _mapper_all.ToImmutableDictionary();
        public ImmutableDictionary<Type, bool> Singletons => _singleton.ToImmutableDictionary();

        public DependenciesConfiguration()
        {

            _mapper     = new ConcurrentDictionary<Type, Type>();
            _mapper_all = new ConcurrentDictionary<Type, IEnumerable<Type>>();
            _singleton  = new ConcurrentDictionary<Type, bool>();
        }

        public bool Has(Type dependencyType, out bool is_singleton)
        {

            is_singleton = Singletons.ContainsKey(dependencyType);
            return Mapper.ContainsKey(dependencyType);
        }

        public Type Get(Type closedDependencyType)
        {

            Type? implementationType;

            if (Mapper.TryGetValue(closedDependencyType, out implementationType))
            {

                return implementationType;
            } 
            else if (closedDependencyType.IsGenericType && Mapper.TryGetValue(closedDependencyType.GetGenericTypeDefinition(), out implementationType)) 
            {
                Type[] typeArguments = closedDependencyType.GetGenericArguments().ToArray();
                return implementationType.MakeGenericType(typeArguments);
            } 
            else throw new DependenciesConfigurationException("No such dependency");
        }

        public IEnumerable<Type> GetAll(Type closedDependencyType)
        {
            IEnumerable<Type>? implementationTypes;

            if (MapperAll.TryGetValue(closedDependencyType, out implementationTypes))
            {

                return implementationTypes;
            } 
            else if (closedDependencyType.IsGenericType && MapperAll.TryGetValue(closedDependencyType.GetGenericTypeDefinition(), out implementationTypes)) 
            {

                return implementationTypes.Select(openDependency => openDependency.MakeGenericType(closedDependencyType));
            } 
            else throw new DependenciesConfigurationException("No such dependency");
        }

        public void Register<TDependency, TImplementation>()
            where TDependency : class
            where TImplementation : TDependency
        {
            var D = typeof(TDependency);
            var I = typeof(TImplementation);
            register(D, I);
        }

        public void Register(Type OpenDependencyGeneric, Type OpenImplementationGeneric)
        {
            var D = OpenDependencyGeneric;
            var I = OpenImplementationGeneric;

            openGenericCheck(D, I);
            register(D, I);
        }

        public void Singleton<TDependency, TImplementation>()
            where TDependency : class
            where TImplementation : TDependency
        {
            var D = typeof(TDependency);
            var I = typeof(TImplementation);
            singleton(D, I);
        }

        public void Singleton(Type OpenDependencyGeneric, Type OpenImplementationGeneric)
        {
            var D = OpenDependencyGeneric;
            var I = OpenImplementationGeneric;

            openGenericCheck(D, I);
            singleton(D, I);
        }

        private protected void register(Type D, Type I)
        {
            _mapper.AddOrUpdate(D, I, (existingKey, existingValue) => I);
            addToMapperAll(D, I);
            _singleton.TryRemove(D, out var removedValue);
        }

        private protected void singleton(Type D, Type I)
        {
            _mapper.AddOrUpdate(D, I, (existingKey, existingValue) => I);
            addToMapperAll(D, I);
            _singleton.GetOrAdd(D, true);
        }

        private protected void addToMapperAll(Type D, Type I)
        {
            IEnumerable<Type> dependencies = _mapper_all.GetOrAdd(D, Enumerable.Empty<Type>());

            if (dependencies == null || !dependencies.Any())
            {
                dependencies = new List<Type> { I };
            }
            else if (!dependencies.Any(x => x == I))
            {
                var updatedDependencies = dependencies.ToList();
                updatedDependencies.Add(I);
                dependencies = updatedDependencies;
            }

            _mapper_all[D] = dependencies;
        }

        private protected void openGenericCheck(Type D, Type I)
        {
            // (D.IsClass || D.IsInterface) - "as self" support
            if (!(D.IsGenericTypeDefinition && (D.IsClass || D.IsInterface) && I.IsGenericTypeDefinition && I.IsClass && !I.IsAbstract))
                throw new DependenciesConfigurationException("Bad register parameters");

            try
            {

                Type closedDependencyGenericType        = D.MakeGenericType(typeof(object));
                Type closedImplementationGenericType    = I.MakeGenericType(typeof(object));

                if (!closedDependencyGenericType.IsAssignableFrom(closedImplementationGenericType))
                    throw new DependenciesConfigurationException("Register: is not assignable");

            }
            catch (ArgumentException ex)
            {

                throw new DependenciesConfigurationException("Register: is not assignable", ex);
            }
        }
    }
}
