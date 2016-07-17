using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    [RoutePrefix("api/default")]
    public class MessageController : ApiController
    {
        [Route("")]
        public string GetMessage()
        {
            return "hello, world";
        }
    }
}
