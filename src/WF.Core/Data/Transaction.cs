using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF.Core.Data
{
    [Serializable]
    [TableName("Transactions")]
    public class Transaction : BusinessObject
    {
        public string DebitAccount { get; set; }
        public string CreditAccount { get; set; }
        public decimal Amount { get; set; }
        public string Narration { get; set; }
        public bool Completed { get; set; }
        public string TransactionRef { get; set; }
        public string TraceID { get; set; }
    }
}
