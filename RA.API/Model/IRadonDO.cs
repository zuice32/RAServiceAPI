namespace RA.microservice.Model
{
    interface IRadonDO
    {
        string type { get; set; }

        string zip { get; set; }

        double average { get; set; }

        uint numberOfTests { get; set; }

        uint limitMin { get; set; }

        uint limitMax { get; set; }
    }
}
