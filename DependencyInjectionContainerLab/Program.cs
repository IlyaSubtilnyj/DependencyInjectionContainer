using DependencyInjectionContainerLib;

namespace DependencyInjectionContainerLab
{

    interface ITest
    {
        public void lol();
    }

    class Test: ITest
    {
        public Test() {}

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

    interface ITestP<T> {
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

    internal class Program
    {
        static void Main(string[] args)
        {
            DependenciesConfiguration configuration = new DependenciesConfiguration();
            configuration.Register<ITest, Test>();
            configuration.Singleton<ITest, Test2>();

            configuration.Register<ITestP<int>, TestP<int>>();

            configuration.Register(typeof(ITestP<>), typeof(TestP<>));

            //configuration.Singleton(typeof(ITestP<>), typeof(TestP<int>));

   
            configuration.Get(typeof(ITestP<string>));
        }
    }
}