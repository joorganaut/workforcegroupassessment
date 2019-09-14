using WF.Core.Contract;
using WF.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF.Core.Model
{
    public class AccountResponse : Response, IHTTPObject
    {
        public List<Account> Accounts { get; set; }
    }
}
