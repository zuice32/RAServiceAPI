using System;
using System.Collections.Generic;
using System.Linq;

namespace RA.WaterRepository
{
    public static class WaterExtensions
    {
        public static List<WaterCharacteristic> orderedList(List<WaterRecord> coll)
        {
            List<WaterCharacteristic> ordered = new List<WaterCharacteristic>();

            //TODO: hardcoding the custom ordering. Revisit later
            var recDis = coll.Where(cl => cl.characteristic_name == "Dissolved oxygen (DO)").FirstOrDefault();
            ordered.Add(new WaterCharacteristic()
            {
                characteristic = "Fishing: *dis oxy",
                count = recDis != null ? recDis.count : 0,
                median = recDis != null ? Math.Round(recDis.median, 2) : 0,
                uom = recDis != null ?
                recDis.result_measure_unit_code != "None" ? recDis.result_measure_unit_code : "" : ""
            });


            var recPh = coll.Where(cl => cl.characteristic_name == "pH").FirstOrDefault();            
            ordered.Add(new WaterCharacteristic()
            {
                characteristic = "Fishing: pH",
                count = recPh != null ? recPh.count : 0,
                median = recPh != null ? Math.Round(recPh.median, 2) : 0,
                uom = recPh != null ?
                recPh.result_measure_unit_code != "None" ? recPh.result_measure_unit_code : "" : ""
            });

            var recSol = coll.Where(cl => cl.characteristic_name == "Total suspended solids").FirstOrDefault();
            ordered.Add(new WaterCharacteristic()
            {
                characteristic = "Swimming: Solids",
                count = recSol != null ? recSol.count : 0,
                median = recSol != null ? Math.Round(recSol.median, 2) : 0,
                uom = recSol != null ?
                recSol.result_measure_unit_code != "None" ? recSol.result_measure_unit_code : "" : ""
            });

            var recBac = coll.Where(cl => cl.characteristic_name == "Fecal Streptococcus Group Bacteria").FirstOrDefault();            
            ordered.Add(new WaterCharacteristic()
            {
                characteristic = "Swimming: Bacteria",
                count = recBac != null ? recBac.count : 0,
                median = recBac != null ? Math.Round(recBac.median, 2) : 0,
                uom = recBac != null ?
                recBac.result_measure_unit_code != "None" ? recBac.result_measure_unit_code : "" : ""
            });

            var recNit = coll.Where(cl => cl.characteristic_name == "Nitrogen").FirstOrDefault();            
            ordered.Add(new WaterCharacteristic()
            {
                characteristic = "Farm Impact: Nitrogen",
                count = recNit != null ? recNit.count : 0,
                median = recNit != null ? Math.Round(recNit.median, 2) : 0,
                uom = recNit != null ?
                recNit.result_measure_unit_code != "None" ? recNit.result_measure_unit_code : "" : ""
            });
            


            var recPho = coll.Where(cl => cl.characteristic_name == "Phosphorus").FirstOrDefault();            
            ordered.Add(new WaterCharacteristic()
            {
                characteristic = "Farm Impact: Phosphorus",
                count = recNit != null ? recPho.count : 0,
                median = recNit != null ? Math.Round(recPho.median, 2) : 0,
                uom = recNit != null ?
                recNit.result_measure_unit_code != "None" ? recNit.result_measure_unit_code : "" : ""
            });
            

            var recIro = coll.Where(cl => cl.characteristic_name == "Iron").FirstOrDefault();
            ordered.Add(new WaterCharacteristic()
            {          
                characteristic = "Mining Impact: Iron",
                count = recIro != null ? recIro.count : 0,
                median = recIro != null ? Math.Round(recIro.median * 0.001, 2) : 0,
                uom = recIro != null ?
                recIro.result_measure_unit_code != "None" ? recIro.result_measure_unit_code : "" : ""
            });
            

            var recMang = coll.Where(cl => cl.characteristic_name == "Manganese").FirstOrDefault();
            ordered.Add(new WaterCharacteristic()
            {
                characteristic = "Mining Impact: Manganese",
                count = recMang != null ? recMang.count : 0,
                median = recMang != null ? Math.Round(recMang.median * 0.001, 2) : 0,
                uom = recMang != null ?
                recMang.result_measure_unit_code != "None" ? recMang.result_measure_unit_code : "" : ""
            });


            var recSul = coll.Where(cl => cl.characteristic_name == "Sulfate").FirstOrDefault();            
            ordered.Add(new WaterCharacteristic()
            {
                characteristic = "Mining Impact: Sulfate",
                count = recSul != null ? recSul.count : 0,
                median = recSul != null ? Math.Round(recSul.median, 2) : 0,
                uom = recSul != null ?
                recSul.result_measure_unit_code != "None" ? recSul.result_measure_unit_code : "" : ""
            });

            var recSp = coll.Where(cl => cl.characteristic_name == "Specific conductance").FirstOrDefault();
            ordered.Add(new WaterCharacteristic()
            {
                characteristic = "Urban Impact: Sp Conduct",
                count = recSp != null ? recSp.count : 0,
                median = recSp != null ? Math.Round(recSp.median, 2) : 0,
                uom = recSp != null ?
                recSp.result_measure_unit_code != "None" ? recSp.result_measure_unit_code : "" : ""
            });
            


            var recChl = coll.Where(cl => cl.characteristic_name == "Chloride").FirstOrDefault();            
            ordered.Add(new WaterCharacteristic()
            {
                characteristic = "Urban Impact: Chlorides",
                count = recChl != null ? recChl.count : 0,
                median = recChl != null ? Math.Round(recChl.median, 2) : 0,
                uom = recChl != null ?
                recChl.result_measure_unit_code != "None" ? recChl.result_measure_unit_code : "" : ""
            });

            return ordered;
        }
    }
}
