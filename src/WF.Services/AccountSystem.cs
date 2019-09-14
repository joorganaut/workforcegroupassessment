using WF.Core.Data;
using WF.Core.Model;
using WF.Core.Models;
using WF.Core.Processors;
using WF.DAO;
using WF.Service.Common;
using NPoco;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WF.Data;

namespace WF.Service
{
    public class AccountSystem
    {
        HTTPConfig postingConfig;
        public AccountSystem()
        {            
            postingConfig = new HTTPConfig();
            postingConfig.DataType = "application/json";           
            postingConfig.Method = HTTPMethod.POST;
        }
        public async Task<Account> CreateNewAccount(Customer customer, AccountCreateRequest details)
        {
            Account result = new Account();
            try
            {
                result.AccountNumber = Guid.NewGuid().ToString();
                result.CustomerID = customer.CifID;
                result.DateCreated = DateTime.Now;
                result.DateLastModified = DateTime.Now;
                result.Currency = details.Currency;
                result.ProductCode = details.ProductCode;
                result = await AccountDAO.SaveAccount(result);
            }
            catch (AggregateException ae)
            {
                result.Error = ae.Message;
            }
            return result;
        }
        public async Task<AccountCreateResponse> CreateAccountResponseAsync(AccountCreateRequest request)
        {
            AccountCreateResponse response = new AccountCreateResponse();
            if (request != null && !string.IsNullOrWhiteSpace(request.CustomerID))
            {
                var Customer = await CustomerDAO.RetrieveByCIfId(request.CustomerID);
                if (Customer != null)
                {
                    var account = await CreateNewAccount(Customer, request);
                    if (account != null)
                    {
                        response.Accounts = new List<Account>();
                        response.Accounts.Add(account);
                        response.ResponseCode = "00";
                        response.ResponseMessage = "Successful";
                    }
                    else
                    {
                        response.ResponseCode = "17";
                        response.ResponseMessage = "Unable to create Account";
                    }
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
        public async Task<Account> GetAccountByNumber(string number)
        {
            Account result = new Account();
            try
            {
                result = await AccountDAO.RetrieveByAccountNumber(number);
            }
            catch (Exception e)
            {
                result.Error = e.Message;
            }
            return result;
        }

        public async Task<AccountResponse> GetAccountResponseAsync(AccountRequest request)
        {
            AccountResponse response = new AccountResponse();
            if (request != null && !string.IsNullOrWhiteSpace(request.AccountNumber))
            {
                var account = await GetAccountByNumber(request.AccountNumber);
                if (account != null && string.IsNullOrWhiteSpace(account.Error))
                {
                    response.Accounts = new List<Account>();
                    response.Accounts.Add(account);
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Successful";
                }
                else
                {
                    response.ResponseCode = "17";
                    response.ResponseMessage = "Invalid Account number";
                }
            }
            else
            {
                response.ResponseCode = "13";
                response.ResponseMessage = "Invalid Request";
            }
            return response; 
        }

        public async Task<AccountResponse> GetAccountsByCustIDResponseAsync(AccountRequest request)
        {
            AccountResponse response = new AccountResponse();
            if (request != null && !string.IsNullOrWhiteSpace(request.CustomerID))
            {
                var account = await GetAccountByCustomerIDAsync(request.CustomerID);
                if (account != null && account.Count > 0)
                {
                    response.Accounts = new List<Account>();
                    response.Accounts.AddRange(account);
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Successful";
                }
                else
                {
                    response.ResponseCode = "17";
                    response.ResponseMessage = "Invalid Customer ID";
                }
            }
            else
            {
                response.ResponseCode = "13";
                response.ResponseMessage = "Invalid Request";
            }
            return response;
        }

        public async Task<List<Account>> GetAccountByCustomerIDAsync(string custID)
        {
            List<Account> result = new List<Account>();
            try
            {
                result = await AccountDAO.RetrieveAccountByCustomerID(custID);
            }
            catch (Exception e)
            {
                result.Add(new Account() { Error = e.Message });
            }
            return result;
        }

    }
}

