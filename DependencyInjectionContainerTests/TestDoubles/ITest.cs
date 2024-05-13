using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionContainerTests.TestDoubles
{
    interface ITest
    {
        public void lol();
    }

    class Test : ITest
    {

        public void lol()
        {
            throw new NotImplementedException();
        }
    }

    class Test2 : ITest
    {
        public void lol()
        {
            throw new NotImplementedException();
        }
    }

    abstract class AbstractTest
    {
        public abstract void lol();
    }

    class Test3 : AbstractTest
    {
        public override void lol()
        {
            throw new NotImplementedException();
        }
    }
}
