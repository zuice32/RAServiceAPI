using RA.MongoDB;
using Core.TypeExtensions;
using System.Drawing;
using System.Collections.Generic;

namespace RA.RadonRepository
{
    public class RadonModel : Entity, IRadonModel
    {
        public string type { get; set; }

        public string uom { get { return "pCi/L"; } }

        public string zip { get; set; }

        public double average { get; set; }

        public double minValue { get; set; }

        public double maxValue { get; set; }

        public double median { get; set; }

        public uint numberOfTests { get; set; }

        public uint limitMin { get { return 4; } }

        public uint limitMax { get { return 20; } }

        public string minColorRGB { get { return color.RGBConverter(Color.Green); } }

        public string maxColorRGB { get { return color.RGBConverter(Color.Red); } }

        public uint year { get; set; }

        public string averageColorRGB { get; set; }
        
        public IEnumerable<uint> year_data { get; set; }

        public IEnumerable<uint> count_data { get; set; }

        public IEnumerable<double> average_data { get; set; }

    }
}
