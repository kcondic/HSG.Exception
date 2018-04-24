using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Owin;

[assembly: OwinStartup(typeof(HSG.Exception.Web.Startup))]
namespace HSG.Exception.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var httpConfig = new HttpConfiguration();

            ConfigureJwtTokenConsumption(app);
            ConfigureWebApi(httpConfig);
            app.UseWebApi(httpConfig);
        }

        private void ConfigureJwtTokenConsumption(IAppBuilder app)
        {
            var issuer = ConfigurationManager.AppSettings["as:Issuer"];
            var audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            var audienceSecret = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["as:AudienceSecret"]);

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audienceId },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
                    }
                });
        }

        private void ConfigureWebApi(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            //Enable CORS to allow frontend/backend communication
            var cors = new EnableCorsAttribute("http://localhost:8080", "*", "*");
            config.EnableCors(cors);

            //Apply the [Authorize] attribute globally
            config.Filters.Add(new AuthorizeAttribute());
        }
    }
}