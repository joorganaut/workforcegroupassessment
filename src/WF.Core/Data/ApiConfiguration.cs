using WF.Core.Contract;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF.Core.Data
{
    [PrimaryKey("ID", AutoIncrement = true)]
    public class ApiConfiguration : BusinessObject
    {
        public virtual string UrlPrefix { get; set; }
    }
    public class ApiConfigurationMap : BusinessObjectMap<ApiConfiguration>
    {
    }
}
