using System;
using System.Net;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RA.DALAccess
{
    public class Client<T> : IClient<T>, IDisposable where T : class
    {
        public Client(string url)
        {
            this.url = url;
        }

        private bool disposed = false;

        public Guid clientID { get; set; }

        public string url { get; set; }

        public string response { get; set; }
        

        public IEnumerable<T> Get(string action)
        {
            var serializer = new JsonSerializer();
            var client = new HttpClient();
            var header = new MediaTypeWithQualityHeaderValue("application/json");
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(header);

            // Note: port number might vary.
            using (var stream = client.GetStreamAsync(action).Result)
            using (var sr = new StreamReader(stream))
            using (var jr = new JsonTextReader(sr))
            {
                while (jr.Read())
                {
                    if (jr.TokenType != JsonToken.StartArray && jr.TokenType != JsonToken.EndArray)
                        yield return serializer.Deserialize<T>(jr);
                }
            }            
        }

        public async Task<bool> CheckConnection()
        {
            bool result = false;

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(url));

                if (response != null || response.StatusCode == HttpStatusCode.OK)
                {
                    result = true;
                }
            }

            return result;
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //TODO: implement client object disposal
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
