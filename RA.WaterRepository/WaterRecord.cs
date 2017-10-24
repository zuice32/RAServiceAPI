using System;
using System.Collections.Generic;
using System.Text;

namespace RA.WaterRepository
{
    public class WaterRecord : IWaterRecord
    {
        public string characteristic_name { get; set; }
        public uint count { get; set; }
        public double median { get; set; }
    }
}
