using System;
using System.Collections.Generic;
using System.Text;
using RA.MongoDB;

namespace RA.WaterRepository
{
    public class WaterModel : Entity, IWaterModel
    {
        public double latitude { get; set; }
        public double longitude { get; set; }

        public string type { get; set; }
        public List<WaterCharacteristic> characteristics { get; set; }
        public List<double> data { get; set; }
        public List<string> characteristic_data { get; set; }
        public int year { get; set; }
        public string location_name { get; set; }
    }
}
