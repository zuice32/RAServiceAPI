using System;


namespace RA.RadonRepository
{
    public interface IRadonRecord
    {
        string radon_data_identifier { get; set; }

        string state_code { get; set; }

        string address_postal_code { get; set; }

        string county_code { get; set; }

        string municipality_name { get; set; }

        int? radon_test_result_identifier { get; set; }

        string radon_data_source_name { get; set; }

        string building_purpose_code { get; set; }

        string building_sub_type_code { get; set; }

        string test_floor_level_test { get; set; }

        string test_method_type_code { get; set; }

        string mitigation_system_indicator { get; set; }

        double? measure_value { get; set; }

        DateTime test_start_date { get; set; }

        DateTime test_end_date { get; set; }
    }
}
