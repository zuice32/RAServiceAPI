using System;
using System.Collections.Generic;

namespace RA.MongoDB
{
    public interface IGenericRepository<T> : IDisposable
    {
        IEnumerable<T> List { get; }
        void Upsert(T entity);
        void Delete(T entity);
    }
}
