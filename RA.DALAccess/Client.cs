using System;
using System.Net;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RA.DALAccess
{
    public class Client : IClient, IDisposable
    {
        public Client(string url)
        {
            this.url = url;
        }

        private bool disposed = false;

        public Guid clientID { get; set; }

        public string url { get; set; }

        public string response { get; set; }
        

        public async Task<string> Get(string action)
        {

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(url);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                
                HttpResponseMessage Res = await client.GetAsync(action);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    return Res.Content.ReadAsStringAsync().Result;
                }
            }

            //else return null
            return null;
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
