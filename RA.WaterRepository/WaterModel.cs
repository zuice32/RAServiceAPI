using System;
using System.Collections.Generic;
using System.Text;
using RA.MongoDB;

namespace RA.WaterRepository
{
    public class WaterModel : Entity, IWaterModel
    {        
        public string type { get; set; }
        public string characteristic { get; set; }
        public uint count { get; set; }
        public double median { get; set; }
        public int year { get; set; }
    }
}
