namespace RA.microservice.Model
{
    public class RadonDO : IRadonDO
    {
        public string type { get; set; }

        public string zip { get; set; }

        public double average { get; set; }

        public uint numberOfTests { get; set; }

        public uint limitMin { get; set; }

        public uint limitMax { get; set; }
    }
}
