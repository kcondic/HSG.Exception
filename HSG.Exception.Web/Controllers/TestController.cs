using System.Web.Http;

namespace HSG.Exception.Web.Controllers
{
    [Route("api/test")]
    public class TestController : ApiController
    {
        [HttpGet]
        public int GetNumberFive()
        {
            return 5;
        }
    }
}
