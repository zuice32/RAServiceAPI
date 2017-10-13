using RA.MongoDB;

namespace RA.microservice.Model
{
    public class Settings : ISetting
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }
}
