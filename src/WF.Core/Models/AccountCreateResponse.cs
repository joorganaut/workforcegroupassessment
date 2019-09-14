using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WF.Core.Contract;
using WF.Core.Data;

namespace WF.Core.Model
{
    public class AccountCreateResponse : Response, IHTTPObject
    {
        public List<Account> Accounts { get; set; }
    }
}
