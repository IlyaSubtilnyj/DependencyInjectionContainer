using DependencyInjectionContainerLib;
using DependencyInjectionContainerTests.TestDoubles;

namespace DependencyInjectionContainerTests
{
    [TestClass]
    public class DependencyInjectionContainerTests
    {
        static IDependenciesConfigurationWrite g_configuration;
        static IDependencyProvider g_provider;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {

            g_configuration = new DependenciesConfiguration();
            //register
            g_provider = new DependencyProvider(g_configuration);
        }

        [TestMethod]
        public void DIConfiguration_WhenNotGenericInterfaceDependency()
        {
            var configuration = new DependenciesConfiguration();
            configuration.Register<ITest, Test>();

            var expectedType    = typeof(Test);
            var actualType      = configuration.Get(typeof(ITest));
            Assert.AreEqual(actualType, expectedType);
        }

        [TestMethod]
        public void DIConfiguration_WhenNotGenericAbstractDependency()
        {
            var configuration = new DependenciesConfiguration();
            configuration.Register<AbstractTest, Test3>();

            var expectedType = typeof(Test3);
            var actualType = configuration.Get(typeof(AbstractTest));
            Assert.AreEqual(actualType, expectedType);
        }

        [TestMethod]
        public void DIConfiguration_WhenNotGenericAsSelfDependency()
        {
            var configuration = new DependenciesConfiguration();
            configuration.Register<Test2, Test2>();

            var expectedType = typeof(Test2);
            var actualType = configuration.Get(typeof(Test2));
            Assert.AreEqual(actualType, expectedType);
        }

        [TestMethod]
        [ExpectedException(typeof(DependenciesConfigurationException))]
        public void DIConfiguration_WhenGenericInterfaceDependency()
        {
            var configuration = new DependenciesConfiguration();
            configuration.Register<ITestP<int>, TestP<int>>();

            var expectedType = typeof(TestP<int>);
            var actualType = configuration.Get(typeof(ITestP<int>));
            Assert.AreEqual(actualType, expectedType);

            configuration.Get(typeof(ITestP<>)); //throws DependenciesConfigurationException
        }

        [TestMethod]
        public void DIConfiguration_WhenOpenGenericDependency()
        {
            var configuration = new DependenciesConfiguration();
            configuration.Register(typeof(ITestP<>), typeof(TestP<>));

            var expectedType = typeof(TestP<int>);
            var actualType = configuration.Get(typeof(ITestP<int>));
            Assert.AreEqual(actualType, expectedType);
        }

        [TestMethod]
        public void DIConfiguration_WhenMultipleDependencies()
        {
            var configuration = new DependenciesConfiguration();
            configuration.Register<ITest, Test>();
            configuration.Register<ITest, Test2>();

            IEnumerable<Type> expected  = new List<Type> { typeof(Test), typeof(Test2) };
            IEnumerable<Type> actual    = configuration.GetAll(typeof(ITest));

            CollectionAssert.AreEquivalent(expected.ToArray(), actual.ToArray());

            var expectedType    = typeof(Test2);
            var actualType      = configuration.Get(typeof(ITest));

            Assert.AreEqual(actualType, expectedType);
        }

        [TestMethod]
        public void DIConfiguration_WhenInstancePerDependency()
        {
            var configuration = new DependenciesConfiguration();
            configuration.Register<ITest, Test>();

            Assert.AreEqual(0, configuration.Singletons.Count);
        }

        [TestMethod]
        public void DIConfiguration_WhenSingletonDependency()
        {
            var configuration = new DependenciesConfiguration();
            configuration.Singleton<ITest, Test>();

            Assert.AreEqual(1, configuration.Singletons.Count);
            Assert.IsTrue(configuration.Singletons.Contains(new KeyValuePair<Type, bool>(typeof(ITest), true)));
        }

        [TestMethod]
        public void DIProvider_WhenDefaultConstructorDependency()
        {
            var configuration   = new DependenciesConfiguration();
            configuration.Register<ITest, Test>();
            var provider        = new DependencyProvider(configuration);

            ITest expected  = new Test();
            ITest actual    = provider.Resolve<ITest>();
 
            Assert.AreEqual(expected, actual);
            Assert.AreNotSame(expected, actual);
        }

        [TestMethod]
        public void DIProvider_WhenSingletonDefaultConstructorDependency()
        {
            var configuration = new DependenciesConfiguration();
            configuration.Singleton<ITest, Test>();
            var provider = new DependencyProvider(configuration);

            ITest expected  = provider.Resolve<ITest>();
            ITest actual    = provider.Resolve<ITest>();

            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        public void DIProvider_WhenSingletonOverwrittenDependency()
        {
            var configuration = new DependenciesConfiguration();
            configuration.Singleton<ITest, Test>();
            configuration.Register<ITest, Test>();
            var provider = new DependencyProvider(configuration);

            ITest expected  = provider.Resolve<ITest>();
            ITest actual    = provider.Resolve<ITest>();

            Assert.AreNotSame(expected, actual);
        }

        [TestMethod]
        public void DIProvider_WhenDependencyConstructorHasResolvableDependencies()
        {
            var configuration = new DependenciesConfiguration();
            configuration.Singleton<ITest, Test>();
            configuration.Register<Test2, Test2>();
            var provider = new DependencyProvider(configuration);

            ITest expected  = provider.Resolve<ITest>();
            ITest actual    = provider.Resolve<ITest>();

            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(DependenciesConfigurationException))]
        public void DIProvider_WhenDependencyConstructorHasNotResolvableDependencies()
        {
            var configuration = new DependenciesConfiguration();
            configuration.Singleton<ITest, Test2>();
            var provider = new DependencyProvider(configuration);

            Assert.IsFalse(provider.IsValid);
            provider.Resolve<ITest>();
        }

        [TestMethod]
        public void DIProvider_WhenEnumerableDependency()
        {
            var configuration = new DependenciesConfiguration();
            configuration.Register<ITest, Test>();
            configuration.Register<ITest, Test1>();
            configuration.Register<Test4, Test4>();
            var provider = new DependencyProvider(configuration);

            Assert.IsTrue(provider.IsValid);

            IEnumerable<ITest> expected = new List<ITest> { new Test(), new Test1() };
            IEnumerable<ITest> actual   = provider.Resolve<Test4>().tests;

            CollectionAssert.AreEquivalent(expected.ToArray(), actual.ToArray());
        }
    }
}