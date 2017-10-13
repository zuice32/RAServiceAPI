using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace RA.MongoDB
{
    public interface IMongoRepository<T,I>
    {
        T Get(I id);

        IEnumerable<T> GetAll();

        T Save(T entity);

        void Delete(I id);
        
        void Delete(T entity);
    }
}
