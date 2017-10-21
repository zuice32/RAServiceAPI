using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.RadonRepository
{
    public interface IRadonModel
    {
        string type { get; set; }

        string zip { get; set; }

        double average { get; set; }

        double median { get; set; }

        uint numberOfTests { get; set; }

        uint limitMin { get; }

        uint limitMax { get; }

        string minColorHex { get; }

        string maxColorHex { get; }

        uint minYear { get; set; }

        uint maxYear { get; set; }

        string averageColorHex { get; set; }
    }
}
