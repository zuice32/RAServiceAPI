using System.Collections.Generic;

namespace Core.Logging
{
    public interface IApplicationLog : ILogWriter
    {
        List<ILogWriter> LogWriters { get; }

        ILogFilterList LogFilters { get; }
    }
}