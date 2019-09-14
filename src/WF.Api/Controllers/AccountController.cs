using WF.Core.Data;
using WF.Core.Model;
using WF.Service;
using WF.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace WF.api.Controllers
{
    
    public class AccountController : ApiController
    {
        AccountSystem aSystem = new AccountSystem();
        [HttpPost]
        [Authorize(Roles = "admin, posting, general")]
        [Route("api/RetrieveAccountDetails")]
        public async Task<IHttpActionResult> GetAccountDetails(AccountRequest request)
        {
            AccountResponse result = new AccountResponse();
            result = await aSystem.GetAccountResponseAsync(request);
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Roles = "admin, posting, general")]
        [Route("api/RetrieveAccountsByCustomer")]
        public async Task<IHttpActionResult> GetAccountsByCustomer(AccountRequest request)
        {
            AccountResponse result = new AccountResponse();
            result = await aSystem.GetAccountResponseAsync(request);
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Roles = "admin, posting, general")]
        [Route("api/CreateAccount")]
        public async Task<IHttpActionResult> CreateAccount(AccountCreateRequest request)
        {
            AccountCreateResponse result = new AccountCreateResponse();
            result = await aSystem.CreateAccountResponseAsync(request);
            return Ok(result);
        }
    }
}
