using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WF.Core.Models;
using WF.DAO;

namespace WF.Services
{
    public class TransactionSystem
    {
        public async Task<TransactionResponse> GetTransactionByRefResponseAsync(TransactionRequest request)
        {
            TransactionResponse response = new TransactionResponse();
            if (request != null && !string.IsNullOrWhiteSpace(request.TransactionRef))
            {
                var transaction = await TransactionDAO.RetrieveTransactionsByReference(request.TransactionRef);
                if (transaction != null && transaction.Count > 0)
                {
                    response.Transactions = new List<Core.Data.Transaction>();
                    response.Transactions.AddRange(transaction);
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Successful";
                }
                else
                {
                    response.ResponseCode = "17";
                    response.ResponseMessage = "Invalid Reference number";
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
