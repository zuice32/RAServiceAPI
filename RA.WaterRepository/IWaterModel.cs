using System;
using System.Collections.Generic;
using System.Text;

namespace RA.WaterRepository
{
    public interface IWaterModel
    {        
        string type { get; set; }        
        List<WaterCharacteristic> characteristics { get; set; }
        List<double> data { get; set; }
        List<string> characteristic_data { get; set; }
        int year { get; set; }
    }
}
