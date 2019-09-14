using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WF.Core.Model;
using WF.Core.Models;
using WF.Service;
using WF.Services;

namespace WF.Api.Controllers
{
    public class ValuesController : ApiController
    {
        AccountSystem aSystem = new AccountSystem();
        [HttpPost]
        //[Authorize(Roles = "admin, posting, general")]
        [Route("guest/RetrieveAccountDetails")]
        public async Task<IHttpActionResult> GetAccountDetails(AccountRequest request)
        {
            AccountResponse result = new AccountResponse();
            result = await aSystem.GetAccountResponseAsync(request);
            return Ok(result);
        }
        CustomerSystem cSystem = new CustomerSystem();
        [HttpPost]
        //[Authorize(Roles = "admin, posting, general")]
        [Route("guest/RetrieveCustomerDetails")]
        public async Task<IHttpActionResult> RetrieveCustomerDetails(CustomerRequest request)
        {
            CustomerResponse result = new CustomerResponse();
            result = await cSystem.GetCustomerResponseAsync(request);
            return Ok(result);
        }
        TransactionSystem tSystem = new TransactionSystem();
        [HttpPost]
        //[Authorize(Roles = "admin, posting, general")]
        [Route("guest/RetrieveTransactionDetails")]
        public async Task<IHttpActionResult> RetrieveTransactionDetailsByRef(TransactionRequest request)
        {
            TransactionResponse result = new TransactionResponse();
            result = await tSystem.GetTransactionByRefResponseAsync(request);
            return Ok(result);
        }
    }
}
