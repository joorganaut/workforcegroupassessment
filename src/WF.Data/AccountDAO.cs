using WF.Core.Data;
using WF.DAO.Common;
using NPoco;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF.DAO
{
    public class AccountDAO
    {
        static NPCoreDAO<Account, long> DAO = new NPCoreDAO<Account, long>("CardConnection");
        public async static Task<Account> SaveTransaction(Account trans)
        {
            return await DAO.SaveAsync(trans);
        }
        public async static Task UpdateTransaction(Account trans)
        {
            await DAO.UpdateAsync(trans, new List<string>());
        }
        public async static Task<Account> Retrieve(long id)
        {
            return await DAO.Retrieve(id);
        }
        public async static Task<Account> RetrieveByAccountNumber(string accountNumber)
        {
            return await DAO.RetrieveDataObjectByParametersAsync(x => x.AccountNumber == accountNumber);
        }
        public async static Task<List<Account>> RetrieveAccountByCustomerID(string customerID)
        {
            return await DAO.RetrieveDataObjectsByParametersAsync(x => x.CustomerID == customerID);
        }
    }
}
