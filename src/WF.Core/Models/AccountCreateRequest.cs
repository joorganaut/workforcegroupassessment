using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WF.Core.Contract;

namespace WF.Core.Model
{
    public class AccountCreateRequest : Request, IHTTPObject
    {
        public string CustomerID { get; set; }
        public string ProductCode { get; set; }
        public string Currency { get; set; }
        public bool IsGL { get; set; }
        public string Error { get; set; }
    }
}
