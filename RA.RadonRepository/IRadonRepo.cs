using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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
    }
}
