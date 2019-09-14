using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WF.Core.Contract;

namespace WF.Core.Data
{
    [PrimaryKey("ID", AutoIncrement = true)]
    public class Customer : BusinessObject
    {
        public string CifID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<Account> Accounts { get; set; }
    }
    public class CustomerMap : BusinessObjectMap<Customer>
    {
    }

   
}
