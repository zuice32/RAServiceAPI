﻿using System.Collections.Generic;

namespace RA.RadonRepository
{
    public interface IRadonModel
    {
        string type { get; set; }

        string uom { get; }

        string zip { get; set; }

        double average { get; set; }

        double median { get; set; }

        double minValue { get; set; }

        double maxValue { get; set; }

        uint numberOfTests { get; set; }

        uint limitMin { get; }

        uint limitMax { get; }

        string minColorRGB { get; }

        string maxColorRGB { get; }

        uint year { get; set; }

        string averageColorRGB { get; set; }

        IEnumerable<uint> year_data { get; set; }

        IEnumerable<uint> count_data { get; set; }

        IEnumerable<double> average_data { get; set; }
    }
}
