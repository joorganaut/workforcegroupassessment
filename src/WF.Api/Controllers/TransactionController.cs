using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WF.Core.Models;
using WF.Services;

namespace WF.Api.Controllers
{
    public class TransactionController : ApiController
    {
        TransactionSystem aSystem = new TransactionSystem();
        [HttpPost]
        [Authorize(Roles = "admin, posting, general")]
        [Route("api/RetrieveTransactionDetails")]
        public async Task<IHttpActionResult> RetrieveTransactionDetailsByRef(TransactionRequest request)
        {
            TransactionResponse result = new TransactionResponse();
            result = await aSystem.GetTransactionByRefResponseAsync(request);
            return Ok(result);
        }
    }
}
