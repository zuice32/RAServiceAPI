using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RA.WaterRepository
{
    public interface IWaterRepo
    {
        WaterModel GetWaterRecord(string id);

        bool WaterModelsExist(Expression<Func<WaterModel, bool>> predicate);

        List<WaterModel> findWaterModels(Expression<Func<WaterModel, bool>> query, string collection);

        IEnumerable<WaterModel> GetAllWaterModels();

        WaterModel SaveWaterModel(WaterModel entity);

        IEnumerable<WaterModel> SaveWaterModels(List<WaterModel> entity);

        void DeleteWaterModel(string id);

        void DeleteWaterModel(WaterModel entity);

        Task<WaterModel> createWaterModel(string url, double latitude, double longitude, int year);
    }
}
