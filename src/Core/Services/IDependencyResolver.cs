namespace Core.Services
{
    public interface IDependencyResolver : IApplicationService
    {
        T Resolve<T>();
    }
}