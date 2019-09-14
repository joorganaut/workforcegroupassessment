using WF.Core.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF.Core.Models
{
    public class AuthenticationResponse : Response, IHTTPObject
    {
        public bool IsAuthenticated { get; set; }
    }
}
