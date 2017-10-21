using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.RadonRepository
{
    public class RadonModel : IRadonModel
    {
        public string type { get; set; }

        public string zip { get; set; }

        public double average { get; set; }

        public double median { get; set; }

        public uint numberOfTests { get; set; }

        public uint limitMin { get { return 4; } }

        public uint limitMax { get { return 20; } }

        public string minColorHex { get { return "#48fc3f"; } }

        public string maxColorHex { get { return "#ff0000"; } }

        public uint minYear { get; set; }

        public uint maxYear { get; set; }

        public string averageColorHex { get; set; }
        
    }
}
