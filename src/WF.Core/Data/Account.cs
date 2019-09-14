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
    [TableName("Accounts")]
    public class Account : BusinessObject
    {
        public virtual string Name { get; set; }
        public virtual string AccountNumber { get; set; }
        public virtual string Type { get; set; }
        public virtual string Currency { get; set; }
        public virtual decimal Balance { get; set; }
        public virtual string CustomerID { get; set; }
        public virtual decimal Lien { get; set; }
        public virtual string ProductCode { get; set; }
        public virtual string ProductName { get; set; }
        public virtual string AccountStatus { get; set; }
    }
    public class AccountMap : BusinessObjectMap<Account>
    {
    }
}
