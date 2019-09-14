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
        public async Task<LienModel> RetrieveAllCardLiens(LienModel request)
        {
            Page<LienDetail> liens = null;
            try
            {
                liens = await AccountDAO.RetrieveAllCardLiensPaged(request.AccountNumber, request.Page, request.PageSize);
                request.Liens = liens.Items;
                request.Page = (int)liens.CurrentPage;
                request.PageSize = (int)liens.ItemsPerPage;
                request.TotalItemCount = (int)liens.TotalItems;
                request.PageCount = (int)liens.TotalPages;
                request.PageNumber = (int)liens.CurrentPage;
            }
            catch (Exception e)
            {
                request.Liens = new List<LienDetail>();
                request.Liens.Add(new LienDetail() { Error = e.Message });
            }
            return request;
        }
        public async Task<LienDetail> RetrieveCardLienByAccountNumber(string accountNumber)
        {
            LienDetail detail = null;
            try
            {
                detail = await AccountDAO.RetrieveCardLienByAccountNumber(accountNumber);
            }
            catch(Exception e)
            {
                detail = new LienDetail();
                detail.Error = e.Message;
            }
            return detail;
        }
        public async Task<List<LienDetail>> RetrieveAllLiensByAccountNumber(string accountNumber)
        {
            List<LienDetail> detail = null;
            try
            {
                detail = await AccountDAO.RetrieveAllLiensByAccountNumber(accountNumber);
            }
            catch (Exception e)
            {
                detail = new List<LienDetail>();
                detail.Add(new LienDetail() { Error = e.Message });
            }
            return detail;
        }

        public async Task<Account> GetAccountByNumber(string number)
        {
            Account result = new Account();
            try
            {
                result = await AccountDAO.RetrieveAccountByAccountNumber(number);
            }
            catch (Exception e)
            {
                result.Error = e.Message;
            }
            return result;
        }
        public async Task<List<Account>> GetAccountByIDAsync(string custID)
        {
            List<Account> result = new List<Account>();
            try
            {
                result = await AccountDAO.RetrieveAccountByCustID(custID);
            }
            catch (Exception e)
            {
                result.Add(new Account() { Error = e.Message });
            }
            return result;
        }

        public async Task<AccountResponse> GetAccountResponseAsync(AccountRequest request)
        {
            AccountResponse result = new AccountResponse();
            try
            {
                if (string.IsNullOrWhiteSpace(request.AccountNumber))
                {
                    result.ResponseCode = "013";
                    result.ResponseMessage = "Invalid Account Number";
                    return result;
                }
                var apiConfig = await new ApiConfigurationSystem().RetrieveByName("Account");
                postingConfig.IP = apiConfig != null ? apiConfig.UrlPrefix : ConfigurationManager.AppSettings["api"];
                postingConfig.Route = "api/RetrieveAccount";
                postingConfig.Headers = HttpRequestMessageExtensions.GetSOAHeaders("Megatron", "decepticons attack");
                var httpResponse = await HTTPEngine<AccountRequest>.PostMessage(request, postingConfig);
                result = await result.LoadModel(httpResponse) as AccountResponse;
                if (result != null && result.Account != null)
                {
                    if(string.IsNullOrWhiteSpace(result.Account.AccountNumber))
                    {
                        result.ResponseCode = "093";
                        result.ResponseMessage = "Invalid Account Number";
                        return result;
                    }
                    result.ResponseCode = "000";
                    result.ResponseMessage = "Successful";
                }
                else
                {
                    result.ResponseCode = "091";
                    var error = await new ErrorMessage().LoadModel(httpResponse);
                    result.ResponseMessage = error != null?error.Error : "Undefined";
                }
            }
            catch (Exception e)
            {
                result.ResponseCode = "097";
                result.ResponseMessage = $"Account Retrieve Failed for account{request.AccountNumber}: {e.Message}";
            }
            return result;
        }
    }
}

