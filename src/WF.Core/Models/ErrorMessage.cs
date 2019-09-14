using WF.Core.Contract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF.Core.Models
{
    public class ErrorMessage : IHTTPObject
    {
        //public string Message { get; set; }
        [JsonProperty("Message")]
        public string Error { get; set; }
    }
}
