using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionContainerLib
{
    internal class ReflectionClass
    {
        private Type _classType;
        private ReflectionConstructor? _ctor; 

        public ReflectionClass(Type classType)
        {

            this._classType = classType;
            this._ctor = null;

            ConstructorInfo[] ctors = this._classType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (ctors.Length != 0)
            {

                this._ctor = new ReflectionConstructor(ctors[0]);
            }
        }

        public ReflectionConstructor? getConstructor()
        {

            return this._ctor;
        }

        public Type ClassType()
        {
            return this._classType;
        }
    }
}
