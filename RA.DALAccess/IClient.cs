using System;
using System.Net;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RA.DALAccess
{
    public interface IClient
    {
        Task<string> Get(string action);

        Task<bool> CheckConnection();

        void Dispose();
    }
}
