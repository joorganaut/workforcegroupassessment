using WF.Core.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF.Core.Models
{
    public class AuthenticationRequest : Request, IHTTPObject
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string Error { get; set; }
    }
}
