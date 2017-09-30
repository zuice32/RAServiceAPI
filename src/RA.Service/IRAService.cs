using System.ServiceModel;
using System.ServiceModel.Web;
using System.IdentityModel.Tokens;

namespace RA.Service
{
    /// <summary>
    /// End point for CRUD over the ecommerce CMS site management
    /// </summary>
    /// 
    
    [ServiceContract]
    public interface IRAService
    {
        /// <summary>
        /// Provides an average for a datetime now and the zip code of the area provided by
        /// geolocation from 
        /// </summary>
        /// <param name="zip"></param>
        /// <param name="datetime"></param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(UriTemplate = "/GetRadonAverage/{zip}~{datetime}",
            ResponseFormat = WebMessageFormat.Json)]
        uint GetRadonAverage(string zip, string datetime);


        [OperationContract]
        [WebGet(UriTemplate = "/Login/{key}",
            ResponseFormat = WebMessageFormat.Json)]
        SecurityToken GetToken(string key);

    }    
}
