using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF.Core.Data
{
    public class Account : BusinessObject
    {
        public string Name { get; set; }
        public string AccountNumber { get; set; }
        public string Type { get; set; }
        public string Currency { get; set; }
        public decimal Balance { get; set; }
        public string CustomerID { get; set; }
        public decimal Lien { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string AccountStatus { get; set; }
    }
}
