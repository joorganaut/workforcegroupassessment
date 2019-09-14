using WF.Core.Data;
using WF.DAO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF.DAO
{
    public class TransactionDAO
    {
        static NPCoreDAO<Transaction, long> DAO = new NPCoreDAO<Transaction, long>("CardConnection");
        public async static Task<Transaction> SaveTransaction(Transaction trans)
        {
            return await DAO.SaveAsync(trans);
        }
        public async static Task UpdateTransaction(Transaction trans)
        {
            await DAO.UpdateAsync(trans, new List<string>());
        }
        public async static Task<Transaction> Retrieve(long id)
        {
            return await DAO.Retrieve(id);
        }
        public async static Task<List<Transaction>> RetrieveUnprocessedTransactions()
        {
            return await DAO.RetrieveDataObjectsByParametersAsync(x => x.Completed == false);
        }
        public async static Task<List<Transaction>> RetrieveTransactionsByAccountNumber(string accountNumber)
        {
            return await DAO.RetrieveDataObjectsByParametersAsync(x => x.SourceAccount == accountNumber);
        }
        public async static Task<List<Transaction>> RetrieveTransactionsByReference(string transactionRef)
        {
            return await DAO.RetrieveDataObjectsByParametersAsync(x => x.TransactionRef == transactionRef);
        }
    }
}
