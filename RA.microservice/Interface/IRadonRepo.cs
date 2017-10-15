using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.RadonRepository
{
    public interface IRadonRepo
    {
        RadonRecord GetRadonRecord(string id);

        IEnumerable<RadonRecord> GetAllRadonRecords();

        RadonRecord SaveRadonRecord(RadonRecord entity);

        IEnumerable<RadonRecord> SaveRadonRecords(List<RadonRecord> entity);

        void DeleteRadonRecord(string id);

        void DeleteRadonRecord(RadonRecord entity);
    }
}
