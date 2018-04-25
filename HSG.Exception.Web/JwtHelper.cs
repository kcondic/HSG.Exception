using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Text;
using System.Web.Helpers;
using HSG.Exception.DAL;
using Jose;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HSG.Exception.Web
{
    public static class JwtHelper
    {
        private static readonly byte[] Secret = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["as:AudienceSecret"]);
        public static string GetJwtToken(User userToGenerateFor)
        {
            var issuer = ConfigurationManager.AppSettings["as:Issuer"];
            var audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            var currentSeconds = Math.Round(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds);
            var payload = new Dictionary<string, string>
            {
                {"iss", issuer},
                {"aud", audienceId},
                {"exp", (currentSeconds + 30).ToString(CultureInfo.InvariantCulture) },
                {"userid", userToGenerateFor.Id.ToString()},
                {"fullname", $"{userToGenerateFor.FirstName} {userToGenerateFor.LastName}"}
            };

            return JWT.Encode(payload, Secret, JwsAlgorithm.HS256);
        }

        public static string GetRefreshToken(string existingToken)
        {
            var decodedToken = JWT.Decode(existingToken, Secret);
            var decodedJObjectToken = (JObject)JsonConvert.DeserializeObject(decodedToken);
            var currentSeconds = Math.Round(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds);
            var expiryTime = decodedJObjectToken["exp"].ToObject<double>();

            if(currentSeconds - expiryTime > 30)
                return null;

            var payload = new Dictionary<string, string>
            {
                {"iss", decodedJObjectToken["iss"].ToString() },
                {"aud", decodedJObjectToken["aud"].ToString()},
                {"exp", (currentSeconds + 30).ToString(CultureInfo.InvariantCulture) },
                {"userid", decodedJObjectToken["userid"].ToString()},
                {"fullname", decodedJObjectToken["fullname"].ToString()}
            };

            return JWT.Encode(payload, Secret, JwsAlgorithm.HS256);
        }
    }
}