using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WF.Core.Models;
using WF.Data;

namespace WF.Services
{
    public class CustomerSystem
    {
        public async Task<CustomerResponse> GetCustomerResponseAsync(CustomerRequest request)
        {
            CustomerResponse response = new CustomerResponse();
            if (request != null && !string.IsNullOrWhiteSpace(request.CustomerID))
            {
                var customer = await CustomerDAO.RetrieveByCIfId(request.CustomerID);
                if (customer != null && string.IsNullOrWhiteSpace(customer.Error))
                {
                    response.Customer = customer;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Successful";
                }
                else
                {
                    response.ResponseCode = "17";
                    response.ResponseMessage = "Invalid Customer number";
                }
            }
            else
            {
                response.ResponseCode = "13";
                response.ResponseMessage = "Invalid Request";
            }
            return response;
        }
    }
}
