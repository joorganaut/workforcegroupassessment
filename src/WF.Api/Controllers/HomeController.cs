using WF.Core.Data;
using WF.Core.Model;
using WF.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace WF.Api.Controllers
{
    public class HomeController : ApiController
    {
        UserSystem uSystem = null;
        public HomeController()
        {
            uSystem = new UserSystem();
        }
        [HttpPost]
        [Route("api/RegisterUser")]
        public async Task<IHttpActionResult> RegisterUser(User request)
        {
            request = await uSystem.RegisterUser(request.Username, request.Password);
            if(request != null && request.IsAuthenticated == true)
            {
                request.Error = "User successfully registered";
            }
            else
            {
                request.Error = string.IsNullOrWhiteSpace(request.Error) ? "User registration failed" : request.Error;
            }
            return Ok(request);
        }
    }
}
