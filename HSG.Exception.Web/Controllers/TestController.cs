using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
