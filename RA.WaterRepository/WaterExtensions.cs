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
                median = recDis != null ? Math.Round(recDis.median, 2) : 0
            });


            var recPh = coll.Where(cl => cl.characteristic_name == "pH").FirstOrDefault();            
            ordered.Add(new WaterCharacteristic()
            {
                characteristic = "Fishing: pH",
                count = recPh != null ? recPh.count : 0,
                median = Math.Round(recPh.median, 2)
            });

            var recSol = coll.Where(cl => cl.characteristic_name == "Total suspended solids").FirstOrDefault();
            ordered.Add(new WaterCharacteristic()
            {
                characteristic = "Swimming: Solids",
                count = recSol != null ? recSol.count : 0,
                median = recSol != null ? Math.Round(recSol.median, 2) : 0
            });

            var recBac = coll.Where(cl => cl.characteristic_name == "Fecal Streptococcus Group Bacteria").FirstOrDefault();            
            ordered.Add(new WaterCharacteristic()
            {
                characteristic = "Swimming: Bacteria",
                count = recBac != null ? recBac.count : 0,
                median = recBac != null ? Math.Round(recBac.median, 2) : 0
            });

            var recNit = coll.Where(cl => cl.characteristic_name == "Nitrogen").FirstOrDefault();            
            ordered.Add(new WaterCharacteristic()
            {
                characteristic = "Farm Impact: Nitrogen",
                count = recNit != null ? recNit.count : 0,
                median = recNit != null ? Math.Round(recNit.median, 2) : 0
            });
            


            var recPho = coll.Where(cl => cl.characteristic_name == "Phosphorus").FirstOrDefault();            
            ordered.Add(new WaterCharacteristic()
            {
                characteristic = "Farm Impact: Phosphorus",
                count = recNit != null ? recPho.count : 0,
                median = recNit != null ? Math.Round(recPho.median, 2) : 0
            });
            

            var recIro = coll.Where(cl => cl.characteristic_name == "Iron").FirstOrDefault();
            ordered.Add(new WaterCharacteristic()
            {          
                characteristic = "Mining Impact: Iron",
                count = recIro != null ? recIro.count : 0,
                median = recIro != null ? Math.Round(recIro.median * 0.001, 2) : 0
            });
            

            var recMang = coll.Where(cl => cl.characteristic_name == "Manganese").FirstOrDefault();
            ordered.Add(new WaterCharacteristic()
            {
                characteristic = "Mining Impact: Manganese",
                count = recMang != null ? recMang.count : 0,
                median = recMang != null ? Math.Round(recMang.median * 0.001, 2) : 0
            });


            var recSul = coll.Where(cl => cl.characteristic_name == "Sulfate").FirstOrDefault();            
            ordered.Add(new WaterCharacteristic()
            {
                characteristic = "Mining Impact: Sulfate",
                count = recSul != null ? recSul.count : 0,
                median = recSul != null ? Math.Round(recSul.median, 2) : 0
            });

            var recSp = coll.Where(cl => cl.characteristic_name == "Specific conductance").FirstOrDefault();
            ordered.Add(new WaterCharacteristic()
            {
                characteristic = "Urban Impact: Sp Conduct",
                count = recSp != null ? recSp.count : 0,
                median = recSp != null ? Math.Round(recSp.median, 2) : 0
            });
            


            var recChl = coll.Where(cl => cl.characteristic_name == "Chloride").FirstOrDefault();            
            ordered.Add(new WaterCharacteristic()
            {
                characteristic = "Urban Impact: Chlorides",
                count = recChl != null ? recChl.count : 0,
                median = recChl != null ? Math.Round(recChl.median, 2) : 0
            });

            return ordered;
        }
    }
}
