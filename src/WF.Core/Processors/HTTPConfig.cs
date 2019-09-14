using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF.Core.Processors
{
    public class HTTPConfig
    {
        public string IP { get; set; }
        public string Route { get; set; }
        public string DataType { get; set; }
        public HTTPMethod Method { get; set; }
        public string SOAPAction { get; set; }
        public KeyValuePair<string, string>[] Headers { get; set; }
    }

    public enum HTTPMethod
    {
        POST = 0,
        GET = 1
    }
}
