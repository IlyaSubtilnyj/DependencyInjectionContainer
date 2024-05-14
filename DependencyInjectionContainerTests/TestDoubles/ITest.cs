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
        int x = 5;
        public void lol()
        {
            throw new NotImplementedException();
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Test other = (Test)obj;
            return x == other.x;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode();
        }
    }

    class Test0 : ITest
    {
        public void lol()
        {
            throw new NotImplementedException();
        }
    }

    class Test1 : ITest
    {
        string y = "test";
        public void lol()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Test1 other = (Test1)obj;
            return y == other.y;
        }

        public override int GetHashCode()
        {
            return y.GetHashCode();
        }
    }

    class Test2 : ITest
    {
        public int x = 5;
        public ITest test;

        public Test2(ITest test)
        {
            this.test = test;
        }

        public void lol()
        {
            throw new NotImplementedException();
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Test2 other = (Test2)obj;
            return x == other.x && test.Equals(other.test);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + x.GetHashCode();
            hash = hash * 23 + test.GetHashCode();
            return hash;
        }
    }

    class Test4 : ITest
    {
        public int x = 5;
        public IEnumerable<ITest> tests;

        public Test4(IEnumerable<ITest> tests)
        {
            this.tests = tests;
        }

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
