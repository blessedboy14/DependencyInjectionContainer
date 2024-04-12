using DependencyInjectionContainer;

public class Start
{
    public static void Main(string[] args)
    {
        var dependencies = new DependencyConfig();
        dependencies.Register<IService, ServiceImpl1>();
        dependencies.Register<IService, ServiceImpl2>();
        dependencies.Register<IRepository, RepositoryImpl>();
        var provider = new DependencyProvider(dependencies);
        IEnumerable<IService> services = provider.Resolve<IEnumerable<IService>>();

        var x = 0;
    }
}

interface IService { }
class ServiceImpl1 : IService
{
    IRepository test;
    public ServiceImpl1(IRepository repository)
    {
        test = repository;
    }
}

class ServiceImpl2 : IService
{
    IRepository test;
    public ServiceImpl2(IRepository repository)
    {
        test = repository;
    }
}

interface IRepository { }
class RepositoryImpl : IRepository
{
    public RepositoryImpl() { }
}
