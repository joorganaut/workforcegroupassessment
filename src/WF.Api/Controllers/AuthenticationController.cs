using WF.Core.Models;
using WF.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace WF.api.Controllers
{
    public class AuthenticationController : ApiController
    {
        AuthenticationSystem aSystem = new AuthenticationSystem();
        [HttpPost]
        [Authorize(Roles = "admin, posting, general")]
        [Route("api/AuthenticateActiveDirectory")]
        public async Task<IHttpActionResult> AuthenticateActiveDirectory(AuthenticationRequest request)
        {
            AuthenticationResponse result = new AuthenticationResponse();
            //result = await aSystem.AuthenticateActiveDirectoryAsync(request);
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Roles = "admin, posting, general")]
        [Route("api/Authenticate2FA")]
        public async Task<IHttpActionResult> Authenticate2FA(AuthenticationRequest request)
        {
            AuthenticationResponse result = new AuthenticationResponse();
            //result = await aSystem.Authenticate2FAAsync(request);
            return Ok(result);
        }
    }
}
