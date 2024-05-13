using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionContainerLib
{
    public class DependencyProvider: IDependencyProvider
    {
        private IDependenciesConfigurationRead _configuration;
        public DependencyProvider(IDependenciesConfigurationRead configuration) 
        {
            _configuration = configuration;
        }

        public bool Has<T>()
        {
            return _configuration.Has(typeof(T), out var _);
        }

        public T Resolve<T>()
        {
            throw new NotImplementedException();
        }
    }
}
