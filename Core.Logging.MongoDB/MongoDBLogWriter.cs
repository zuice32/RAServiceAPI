namespace Core.Logging.MongoDB
{
    public class MongoDBLogWriter : ILogWriter
    {
        private readonly ILogRepository _logRepository;

        public MongoDBLogWriter(ILogRepository logRepository )
        {
            _logRepository = logRepository;
        }

        public void AddEntry(ILogEntry logEntry)
        {
            _logRepository.Add(logEntry);
        }
    }
}
