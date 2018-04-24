using System.Web.Http;

namespace HSG.Exception.Web.Controllers
{
    [RoutePrefix("api/test")]
    public class TestController : ApiController
    {
        [HttpGet]
        [Route("")]
        public int GetNumberFive()
        {
            return 5;
        }
    }
}
