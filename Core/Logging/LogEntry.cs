
using System;

namespace Core.Logging
{
    public class LogEntry : ILogEntry
    {
        public LogEntry(MessageLevel messageLevel, string source, string message, DateTime time)
        {
            this.Level = messageLevel;
            this.Source = source;
            this.Message = message;
            this.Time = time;
        }

        public MessageLevel Level { get; set; }

        public string Source { get; set; }

        public string Message { get; set; }

        public DateTime Time { get; set; }
    }
}