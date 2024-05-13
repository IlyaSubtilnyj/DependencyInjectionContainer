using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionContainerLib
{

    public class DependenciesConfigurationException : ApplicationException
    {
        public DependenciesConfigurationException()
        {
        }

        public DependenciesConfigurationException(string message)
            : base(message)
        {
        }

        public DependenciesConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
