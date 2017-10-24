namespace RA.RadonRepository
{
    public interface IRadonModel
    {
        string type { get; set; }

        string zip { get; set; }

        double average { get; set; }

        double median { get; set; }

        uint numberOfTests { get; set; }

        uint limitMin { get; }

        uint limitMax { get; }

        string minColorRGB { get; }

        string maxColorRGB { get; }

        uint minYear { get; set; }

        uint maxYear { get; set; }

        string averageColorRGB { get; set; }
    }
}
