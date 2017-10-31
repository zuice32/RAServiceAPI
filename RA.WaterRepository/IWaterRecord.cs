namespace RA.WaterRepository
{
    public interface IWaterRecord
    {
        string characteristic_name { get; set; }
        uint count { get; set; }
        double median { get; set; }
        double latitude { get; set; }
        double longitude { get; set; }
        string location_name { get; set; }
    }
}
