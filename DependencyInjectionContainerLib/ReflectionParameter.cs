using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionContainerLib
{
    internal class ReflectionParameter
    {
        private ParameterInfo _param;

        public ReflectionParameter(ParameterInfo parameter)
        {

            this._param = parameter;
        }

        public Type ParameterType()
        {

            return _param.ParameterType;
        }
    }
}
