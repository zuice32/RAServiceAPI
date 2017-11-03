namespace RA.WaterRepository
{
    public interface IWaterRecord
    {
        string characteristic_name { get; set; }
        string result_measure_unit_code { get; set; }
        uint count { get; set; }
        double median { get; set; }
        double latitude { get; set; }
        double longitude { get; set; }
        string location_name { get; set; }
    }
}
