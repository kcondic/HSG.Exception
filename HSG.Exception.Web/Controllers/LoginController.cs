using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using HSG.Exception.DAL.Repositories;
using Newtonsoft.Json.Linq;

namespace HSG.Exception.Web.Controllers
{
    [AllowAnonymous]
    [Route("api/login")]
    public class LoginController : ApiController
    {
        public LoginController()
        {
         _userRepository = new UserRepository(ConnectionFactory.GetConnection(ConfigurationManager.ConnectionStrings["UserConnection"].ToString()));   
        }
        private readonly UserRepository _userRepository;

        [HttpPost]
        public IHttpActionResult Login([FromBody]JObject userCredentials)
        {
            if (userCredentials["userName"] == null || userCredentials["password"] == null)
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.NotFound));

            var userName = userCredentials["userName"].ToObject<string>();
            var password = userCredentials["password"].ToObject<string>();

            var user = _userRepository.GetByUserName(userName);
            if (user == null) return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.NotFound));

            //var areCredentialsValid = HashHelper.ValidatePassword(password, user.Password);
            //if (!areCredentialsValid) return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Unauthorized));

            return Ok(JwtHelper.GetJwtToken(user));
        }
    }
}
