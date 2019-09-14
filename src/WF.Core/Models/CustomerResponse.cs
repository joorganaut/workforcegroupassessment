using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WF.Core.Contract;
using WF.Core.Data;

namespace WF.Core.Models
{
    public class CustomerResponse : Response, IHTTPObject
    {
        public Customer Customer { get; set; }
    }
}
