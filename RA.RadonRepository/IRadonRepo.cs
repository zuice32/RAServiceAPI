using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RA.RadonRepository
{
    public interface IRadonRepo
    {
        RadonModel GetRadonRecord(string id);

        List<RadonModel> findRadon(Expression<Func<RadonModel, bool>> query, string collection);

        IEnumerable<RadonModel> GetAllRadonModel();

        RadonModel SaveRadonModel(RadonModel entity);

        IEnumerable<RadonModel> SaveRadonModels(List<RadonModel> entity);

        void DeleteRadonModel(string id);

        void DeleteRadonModel(RadonModel entity);
        
        Task<RadonModel> createRadonModel(string url, string zip, int year);
    }
}
