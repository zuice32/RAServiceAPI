
namespace Core.Logging
{
    public interface ILogWriter
    {
        void AddEntry(ILogEntry logEntry);
    }
}