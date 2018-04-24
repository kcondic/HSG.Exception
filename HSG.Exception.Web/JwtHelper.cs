using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Text;
using HSG.Exception.DAL;
using Jose;

namespace HSG.Exception.Web
{
    public static class JwtHelper
    {
        private static readonly string Issuer = ConfigurationManager.AppSettings["as:Issuer"];
        private static readonly string AudienceId = ConfigurationManager.AppSettings["as:AudienceId"];
        private static readonly byte[] Secret = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["as:AudienceSecret"]);
        public static string GetJwtToken(User userToGenerateFor)
        {
            var currentSeconds = Math.Round(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds);
            var payload = new Dictionary<string, string>
            {
                {"iss", Issuer},
                {"aud", AudienceId},
                {"exp", (currentSeconds + 30).ToString(CultureInfo.InvariantCulture) },
                {"userid", userToGenerateFor.Id.ToString()},
                {"fullname", $"{userToGenerateFor.FirstName} {userToGenerateFor.LastName}"}
            };

            return JWT.Encode(payload, Secret, JwsAlgorithm.HS256);
        }

        public static string GetRefreshToken(string existingToken)
        {
            var decodedToken = JWT.Decode(existingToken, Secret);
            return null;
        }
    }
}