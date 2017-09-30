namespace Core.Application
{
    public interface ICoreIdentity
    {
        string ID { get; }

        string PathToCoreDataDirectory { get; }
    }
}