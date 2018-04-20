using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Text;
using HSG.Exception.DAL;
using Jose;

namespace HSG.Exception.Web
{
    public static class JwtHelper
    {
        public static string GetJwtToken(User userToGenerateFor)
        {
            var issuer = ConfigurationManager.AppSettings["as:Issuer"];
            var audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            var secret = ConfigurationManager.AppSettings["as:AudienceSecret"];
            var currentSeconds = Math.Round(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds);
            var payload = new Dictionary<string, string>
            {
                {"iss", issuer},
                {"aud", audienceId},
                {"exp", (currentSeconds + 3600).ToString(CultureInfo.InvariantCulture) },
                {"userid", userToGenerateFor.Id.ToString()},
                {"fullname", $"{userToGenerateFor.FirstName} {userToGenerateFor.LastName}"},
            };

            return JWT.Encode(payload, Encoding.UTF8.GetBytes(secret), JwsAlgorithm.HS256);
        }
    }
}