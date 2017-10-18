using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RA.MongoDB;
using MongoDB.Bson.Serialization.Attributes;

namespace RA.RadonRepository
{
    public class RadonRecord : Entity, IRadonRecord 
    {
        public string radon_data_identifier { get; set; }

        public string state_code { get; set; }

        public string address_postal_code { get; set; }

        public string county_code { get; set; }

        public string municipality_name { get; set; }

        public int? radon_test_result_identifier { get; set; }

        public string radon_data_source_name { get; set; }

        public string building_purpose_code { get; set; }

        public string building_sub_type_code { get; set; }

        public string test_floor_level_test { get; set; }

        public string test_method_type_code { get; set; }

        public string mitigation_system_indicator { get; set; }

        public double? measure_value { get; set; }

        public DateTime? test_start_date { get; set; }

        public DateTime? test_end_date { get; set; }
    }
}
