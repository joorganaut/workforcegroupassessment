using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WF.Core.Contract;
using WF.Core.Enums;

namespace WF.Core.Data
{
    [PrimaryKey("ID", AutoIncrement = true)]
    public class Transaction : BusinessObject
    {
        public string SourceAccount { get; set; }
        public string DestinationAccount { get; set; }
        public decimal Amount { get; set; }
        public string Narration { get; set; }
        public bool Completed { get; set; }
        public string TransactionRef { get; set; }
        public string TraceID { get; set; }
        public TransactionType Type { get; set; }
    }
    public class TransactionMap : BusinessObjectMap<Transaction>
    {
    }
}
