using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IdentityModel.Tokens;
using System.Security.Claims;

namespace RA.Service
{
    public class RAService : IRAService
    {
        public static RadonRepository.RadonRepository _radonRepo;
        
        public uint GetRadonAverage(string zip, string datetime)
        {
            return 0;
        }

        public SecurityToken GetToken(string token) {


            //TODO: need to finish the JWT token exchange
            System.Configuration.Configuration rootWebConfig =
                System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(null);

            var plainTextSecurityKey = token;

            var signingKey = new InMemorySymmetricSecurityKey(
                Encoding.UTF8.GetBytes(plainTextSecurityKey));

            var signingCredentials = new SigningCredentials(signingKey,
                SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);

            var claimsIdentity = new ClaimsIdentity(new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, rootWebConfig.AppSettings.Settings["NameIdentifier"].Value),
                new Claim(ClaimTypes.Role, rootWebConfig.AppSettings.Settings["Role"].Value),
            }, "Custom");

            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                AppliesToAddress = rootWebConfig.AppSettings.Settings["appliesToAddress"].Value,
                TokenIssuerName = rootWebConfig.AppSettings.Settings["TokenIssuerName"].Value,
                Subject = claimsIdentity,
                SigningCredentials = signingCredentials,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var plainToken = tokenHandler.CreateToken(securityTokenDescriptor);
            var signedAndEncodedToken = tokenHandler.WriteToken(plainToken);

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidAudiences = new string[]
                {
                    rootWebConfig.AppSettings.Settings["appliesToAddress"].Value
                },
                ValidIssuers = new string[]
                {
                    rootWebConfig.AppSettings.Settings["TokenIssuerName"].Value
                },
                IssuerSigningKey = signingKey
            };

            SecurityToken validatedToken;
            tokenHandler.ValidateToken(signedAndEncodedToken,
            tokenValidationParameters, out validatedToken);
            Console.WriteLine(validatedToken.ToString());
            Console.ReadLine();

            return validatedToken;
        }
        
    }
}