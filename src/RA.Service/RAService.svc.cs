using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RA.MongoDB;
using System.IdentityModel.Tokens;
using System.Security.Claims;

namespace RA.Service
{
    public class RAService : IRAService
    {
        public static RadonRepository _radonRepo;
        
        public SiteMap GetData()
        {
         
        }

        public SecurityToken GetToken(string token) {
            
            
        }
        
    }
}