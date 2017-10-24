namespace RA.MongoDB
{
    public interface ISetting
    {
        string ConnectionString { get; set; }
        string Database { get; set; }
        string username { get; set; }
        string pass { get; set; }
        int port { get; set; }
        string url { get; set; }
    }
}
