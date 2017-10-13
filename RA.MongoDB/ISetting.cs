using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.MongoDB
{
    public interface ISetting
    {
        string ConnectionString { get; set; }
        string Database { get; set; }
    }
}
