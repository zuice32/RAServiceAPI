using System;

namespace Core.Logging
{
    public class DebugLogWriter : ILogWriter
    {
        public void AddEntry(ILogEntry entry)
        {
            Console.WriteLine(TextLogEntryFormatter.GetEntry(entry.Level, entry.Source, entry.Message));
        }
    }
}