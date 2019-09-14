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
    [TableName("Customers")]
    public class Customer : BusinessObject
    {
        public virtual string CifID { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string MiddleName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual List<Account> Accounts { get; set; }
    }
    public class CustomerMap : BusinessObjectMap<Customer>
    {
    }

   
}
