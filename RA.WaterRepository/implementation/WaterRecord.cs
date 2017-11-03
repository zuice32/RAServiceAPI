using System;
using System.Collections.Generic;
using System.Text;

namespace RA.WaterRepository
{
    public class WaterRecord : IWaterRecord
    {
        public string characteristic_name { get; set; }
        public string result_measure_unit_code { get; set; }
        public uint count { get; set; }
        public double median { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string location_name { get; set; }
    }
}
