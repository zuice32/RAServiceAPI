namespace RA.MongoDB
{
    public class Settings : ISetting
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string username { get; set; }
        public string pass { get; set; }
        public int port { get; set; }
        public string url { get; set; }
    }
}