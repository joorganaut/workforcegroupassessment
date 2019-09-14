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
    public class AuditTrail : BusinessObject
    {
        public virtual long UserID { get; set; }
        public virtual string Action { get; set; }
        public virtual string IP { get; set; }
        public virtual string Request { get; set; }
        public virtual string Response { get; set; }
        public virtual DateTime RequestTime { get; set; }
        public virtual DateTime ResponseTime { get; set; }
    }
    public class AuditTrailMap : BusinessObjectMap<AuditTrail>
    { }
}
