using System.Threading.Tasks;
using System.Collections.Generic;

namespace RA.DALAccess
{
    public interface IClient<T>
    {
        IEnumerable<T> Get(string action);

        Task<bool> CheckConnection();

        void Dispose();
    }
}
