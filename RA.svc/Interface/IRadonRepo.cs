using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RA.RadonRepository
{
    public interface IRadonRepo
    {
        RadonRecord GetRadonRecord(string id);

        List<RadonRecord> findRadon(Expression<Func<RadonRecord, bool>> query, string collection);

        IEnumerable<RadonRecord> GetAllRadonRecords();

        RadonRecord SaveRadonRecord(RadonRecord entity);

        IEnumerable<RadonRecord> SaveRadonRecords(List<RadonRecord> entity);

        void DeleteRadonRecord(string id);

        void DeleteRadonRecord(RadonRecord entity);
    }
}
