﻿namespace RA.microservice.Model
{
    interface IRAappDO
    {
        string type { get; set; }

        string zip { get; set; }

        double average { get; set; }

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
