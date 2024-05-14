using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace DependencyInjectionContainerLib
{
    public class DependencyProvider: IDependencyProvider
    {
        private IDependenciesConfigurationRead _configuration;
        private ConcurrentDictionary<Type, object> _singletons;
        private bool _is_valid;
        public bool IsValid => _is_valid;
        public DependencyProvider(IDependenciesConfigurationRead configuration) 
        {
            _configuration  = configuration;
            _singletons     = new ConcurrentDictionary<Type, object>();
            IsValidConfiguration(configuration);
        }

        public bool Has<T>()
        {
            return _is_valid && _configuration.Has(typeof(T), out var _);
        }

        public T Resolve<T>()
        {
            if (_is_valid == false)
                throw new DependenciesConfigurationException("Bad configuration given");

            T constructedDependency = (T)Get(typeof(T));
            return constructedDependency;
        }

        private protected void IsValidConfiguration(IDependenciesConfigurationRead configuration)
        {
            _is_valid = true;

            try
            {

                foreach (var kvp in configuration.MapperAll)
                {
                    if (!kvp.Key.IsGenericTypeDefinition)
                        Get(kvp.Key);
                }
            } catch (DependenciesConfigurationException)
            {

                _is_valid = false;
            }
        }

        public object Get(Type id)
        {
            var isIEnumerable = typeof(IEnumerable).IsAssignableFrom(id);

            if (isIEnumerable)
            {
                
                return getEnumerable(id);
            } else
            {
                return getNotEnumerable(id);
            }
        }

        private protected object getNotEnumerable(Type id)
        {
            if (_configuration.Has(id, out var is_singleton))
            {
                var implementationType = _configuration.Get(id);
                return retrieve(implementationType, is_singleton);
            }
            else
                throw new DependenciesConfigurationException($"No such dependency requested: {id}");
        }

        private protected object retrieve(Type implementationType, bool is_singleton)
        {
            if (is_singleton)
            {

                return this.fromSingletons(implementationType);
            }
            else
            {

                return this.prepareObject(implementationType);
            }
        }

        private protected object getEnumerable(Type id)
        {
            var iEnumerableGenericParam = id.GetGenericArguments()[0];
            if (_configuration.Has(iEnumerableGenericParam, out var is_singleton))
            {
                Type listType       = typeof(List<>).MakeGenericType(iEnumerableGenericParam);
                IList? list         = Activator.CreateInstance(listType) as IList;
                var allDependencies = _configuration.MapperAll[iEnumerableGenericParam];

                foreach (var dependency in allDependencies)
                {
                    list!.Add(retrieve(dependency, is_singleton));
                }
                return list!;

            }
            else throw new DependenciesConfigurationException($"No such dependency requested: {id}");
        }

        private protected object prepareObject(Type implementationType)
        {
            object result;

            var classReflector          = new ReflectionClass(implementationType);
            var constructorReflector    = classReflector.getConstructor();
            var constructorParams       = constructorReflector?.getParameters();

            if (constructorReflector == null || constructorParams == null)
            {

                result = ReflectionConstructor.ExtConstructor(classReflector.ClassType());
            }
            else
            {

                List<object> args = new();
                foreach (ReflectionParameter parameter in constructorParams)
                {

                    var ctorDependency = Get(parameter.ParameterType());
                    args.Add(ctorDependency);
                }

                result = constructorReflector.Execute(args.ToArray());
            }

            return result;
        }

        private protected object fromSingletons(Type id)
        {

            if (_singletons.TryGetValue(id, out var obj)) { return obj; }
            else
            {
                //even if singleton has not singleton constructor parameters it must be created once
                object singleton    = prepareObject(id);
                _singletons[id]     = singleton;
                return singleton;
            }
        }
    }
}
