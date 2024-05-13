using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionContainerTests.TestDoubles
{
    interface ITestP<T>
    {
        public void lol();
    }

    class TestP<T> : ITestP<T>
    {
        public void lol()
        {
            throw new NotImplementedException();
        }
    }

    class TestP2<T> : ITestP<T>
    {
        public void lol()
        {
            throw new NotImplementedException();
        }
    }
}
