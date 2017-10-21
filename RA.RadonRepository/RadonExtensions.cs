using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.RadonRepository
{
    public static class RadonExtensions
    {
        public static int determineAverageBucket(int[] parts, double average)
        {
            return 
            average > parts[0] && average < parts[1] ?  1 :
            average > parts[1] && average < parts[2] ?  2 :
            average > parts[2] && average < parts[3] ?  3 : 0;
        }
    }
}
