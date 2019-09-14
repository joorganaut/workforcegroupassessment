using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WF.Core.Contract;

namespace WF.Core.Models
{
    public class TransactionResponse : Response, IHTTPObject
    {
        public List<WF.Core.Data.Transaction> Transactions { get; set; }
    }
}
