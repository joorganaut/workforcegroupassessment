using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WF.Core.Contract;

namespace WF.Core.Models
{
    public class CustomerRequest : Request, IHTTPObject
    {
        public string CustomerID { get; set; }
        public string Error { get; set; }
    }
}
