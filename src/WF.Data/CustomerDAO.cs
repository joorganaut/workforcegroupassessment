using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WF.Core.Data;
using WF.DAO.Common;

namespace WF.Data
{
    public class CustomerDAO
    {
        static NPCoreDAO<Customer, long> DAO = new NPCoreDAO<Customer, long>("CardConnection");
        public async static Task<Customer> SaveCustomer(Customer trans)
        {
            return await DAO.SaveAsync(trans);
        }
        public async static Task UpdateCustomer(Customer trans)
        {
            await DAO.UpdateAsync(trans, new List<string>());
        }
        public async static Task<Customer> Retrieve(long id)
        {
            return await DAO.Retrieve(id);
        }
        public async static Task<Customer> RetrieveByCIfId(string cif)
        {
            return await DAO.RetrieveDataObjectByParametersAsync(x => x.CifID == cif);
        }
    }
}
