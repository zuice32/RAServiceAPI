using System;
using System.Collections.Generic;
using System.Text;

namespace RA.WaterRepository
{
    public interface IWaterModel
    {        
        string type { get; set; }        
        string characteristic { get; set; }
        uint count { get; set; }
        double median { get; set; }
        int year { get; set; }
    }
}
