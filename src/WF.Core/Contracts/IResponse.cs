using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF.Core.Contract
{
    public interface IResponse
    {
        string ResponseCode { get; set; }
        string ResponseMessage { get; set; }
        string Error { get; set; }
    }
    [Serializable]
    public abstract class Response : IResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string Error { get; set; }
    }
}
