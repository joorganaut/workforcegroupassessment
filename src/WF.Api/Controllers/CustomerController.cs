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
    public class CustomerController : ApiController
    {
        CustomerSystem aSystem = new CustomerSystem();
        [HttpPost]
        [Authorize(Roles = "admin, posting, general")]
        [Route("api/RetrieveCustomerDetails")]
        public async Task<IHttpActionResult> RetrieveCustomerDetails(CustomerRequest request)
        {
            CustomerResponse result = new CustomerResponse();
            result = await aSystem.GetCustomerResponseAsync(request);
            return Ok(result);
        }
    }
}
