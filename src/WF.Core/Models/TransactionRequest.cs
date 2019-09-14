using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WF.Core.Contract;

namespace WF.Core.Models
{
    public class TransactionRequest : Request, IHTTPObject
    {
        public string AccountNumber { get; set; }
        public string TransactionRef { get; set; }
        public string Error { get; set; }
    }
}
