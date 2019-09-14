using WF.Core.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF.Core.Model
{
    public class AccountRequest : Request, IHTTPObject
    {
        public string AccountNumber { get; set; }
        public bool IsGL { get; set; }
        public string Error { get; set ; }
    }
}
