using DependencyInjectionContainer;

namespace ContainerTesting
{

    interface ITest { }

    class TestClass1 : ITest
    {

    }

    interface IRepo { }

    class Repo : IRepo 
    { 
          
    }

    class TestClass2 : ITest 
    {
        public IRepo repo;
        public TestClass2(IRepo repo)
        {   
            this.repo = repo;
        }
    }

    interface INew { }

    class New : INew 
    { 

    }

    class TestClass3 : ITest
    {
        public IRepo repo;
        public IService<IRepo> service;
        public INew test;

        public TestClass3(IRepo repo, IService<IRepo> service, INew test)
        {
            this.repo = repo;
            this.service = service;
            this.test = test;
        }
    }

    interface IService<TRepo> where TRepo : IRepo { }

    class ServiceImpl<TRepo> : IService<TRepo> where TRepo : IRepo
    {
        public TRepo repo;
        public ServiceImpl(TRepo repo)
        {
            this.repo = repo;
        }
    }

    public class UnitTest1
    {
        [Fact]
        public void CheckFunctionality_OnOneDependency()
        {
            var config = new DependencyConfig();

            config.Register<ITest, TestClass1>();

            var provider = new DependencyProvider(config);

            var implementatiion = provider.Resolve<ITest>();

            Assert.NotNull(implementatiion);
            Assert.True(implementatiion is TestClass1);
        }

        [Fact]
        public void CheckCreation_WithInnerDependency()
        {
            var config = new DependencyConfig();

            config.Register<ITest, TestClass2>();
            config.Register<IRepo, Repo>();

            var provider = new DependencyProvider(config);

            var implementatiion = provider.Resolve<ITest>();

            Assert.NotNull(implementatiion);
            Assert.True(implementatiion is TestClass2);
            Assert.True((implementatiion as TestClass2).repo is Repo);
        }

        [Fact]
        public void CheckCreation_WithMultipleImplementations()
        {
            var config = new DependencyConfig();

            config.Register<ITest, TestClass1>();
            config.Register<ITest, TestClass2>();
            config.Register<IRepo, Repo>();

            var provider = new DependencyProvider(config);

            var implementatiion = provider.Resolve<IEnumerable<ITest>>();

            Assert.NotNull(implementatiion);
            Assert.True(implementatiion.First() is TestClass1);
            Assert.True((implementatiion.Last() as TestClass2).repo is Repo);
        }

        [Fact]
        public void CheckResult_WithGenericInterface()
        {
            var config = new DependencyConfig();

            config.Register<IRepo, Repo>();
            config.Register<IService<IRepo>, ServiceImpl<IRepo>>();

            var provider = new DependencyProvider(config);

            var implementatiion = provider.Resolve<IService<IRepo>>();

            Assert.NotNull(implementatiion);
            Assert.True(implementatiion is ServiceImpl<IRepo>);
            Assert.True((implementatiion as ServiceImpl<IRepo>).repo is Repo);
        }

        [Fact]
        public void CheckResult_WithMultipleParams_InImplementation()
        {
            var config = new DependencyConfig();

            config.Register<ITest, TestClass3>();
            config.Register<IService<IRepo>, ServiceImpl<IRepo>>();
            config.Register<IRepo, Repo>();
            config.Register<INew, New>();

            var provider = new DependencyProvider(config);

            var implementatiion = provider.Resolve<ITest>();

            Assert.NotNull(implementatiion);
            Assert.True(implementatiion is TestClass3);
            Assert.True((implementatiion as TestClass3).repo is Repo);
            Assert.True((implementatiion as TestClass3).test is New);
            Assert.True((implementatiion as TestClass3).service is ServiceImpl<IRepo>);
        }
    }
}