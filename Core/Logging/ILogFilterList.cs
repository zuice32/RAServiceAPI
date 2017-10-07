using System.Collections.Generic;

namespace Core.Logging
{
    public interface ILogFilterList : IList<ILogFilter>, ILogFilter
    {
    }
}