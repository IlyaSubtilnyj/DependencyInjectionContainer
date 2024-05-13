using DependencyInjectionContainerLib;
using DependencyInjectionContainerTests.TestDoubles;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using static System.Net.Mime.MediaTypeNames;

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
        public void DIProvider_WhenInstancePerDependency()
        {
            var configuration = new DependenciesConfiguration();
            //register

        }

        [TestMethod]
        public void DIProvider_WhenSingletonDependency()
        {
            var configuration = new DependenciesConfiguration();
            //register

        }
    }
}