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
    [TableName("Transactions")]
    public class Transaction : BusinessObject
    {
        public virtual string SourceAccount { get; set; }
        public virtual string DestinationAccount { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual string Narration { get; set; }
        public virtual bool Completed { get; set; }
        public virtual string TransactionRef { get; set; }
        public virtual string TraceID { get; set; }
        public virtual TransactionType Type { get; set; }
    }
    public class TransactionMap : BusinessObjectMap<Transaction>
    {
    }
}
