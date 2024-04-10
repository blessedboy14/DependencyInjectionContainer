using DependencyInjectionContainer;

public class Start
{
    public static void Main(string[] args)
    {
        var dependencies = new DependencyConfig();
        dependencies.Register<IService, ServiceImpl>();
        dependencies.Register<IRepository, RepositoryImpl>();

        var provider = new DependencyProvider(dependencies);

        var service1 = provider.Resolve<IService>();

        var x = 0;
    }
}

interface IService { }
class ServiceImpl : IService
{
    IRepository test;
    public ServiceImpl(IRepository repository)
    {
        test = repository;
    }
}

interface IRepository { }
class RepositoryImpl : IRepository
{
    public RepositoryImpl() { }
}
