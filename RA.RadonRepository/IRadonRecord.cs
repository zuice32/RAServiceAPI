using System;
using MongoDB.Bson.Serialization.Attributes;


namespace RA.RadonRepository
{
    public interface IRadonRecord
    {
        [BsonId]
        string radon_data_identifier { get; set; }

        string state_code { get; set; }

        string address_postal_code { get; set; }

        string county_code { get; set; }

        string municipality_name { get; set; }

        uint radon_test_result_identifier { get; set; }

        string radon_data_source_name { get; set; }

        string building_purpose_code { get; set; }

        string test_floor_level_test { get; set; }

        string mitigation_system_indicator { get; set; }

        uint measure_value { get; set; }

        DateTime test_start_date { get; set; }

        DateTime test_end_date { get; set; }
    }
}
