namespace RA.WaterRepository
{
    public interface IWaterRecord
    {
        string characteristic_name { get; set; }
        uint count { get; set; }
        double median { get; set; }
    }
}
