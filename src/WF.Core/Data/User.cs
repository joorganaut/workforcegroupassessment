using WF.Core.Contract;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace WF.Core.Data
{
    [PrimaryKey("ID", AutoIncrement = true)]
    public class User : BusinessObject
    {
        public virtual string Username { get; set; }
        public virtual string FullName { get; set; }
        public virtual string Password { get; set; }
        public virtual string Error { get; set; }
        public virtual bool IsAuthenticated { get; set; }
        public virtual DateTime LastLoginDate { get; set; }
        public virtual int NumberOfFailedAttempts { get; set; }
        public virtual string Email { get; set; }
        public virtual List<UserRole> Roles { get; set; }
        public virtual string ApprovedIP { get; set; }
    }
    public class UserMap : BusinessObjectMap<User>
    {
    }

    public class PrincipalUser : IPrincipal
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Error { get; set; }
        public bool IsAuthenticated { get; set; }
        public DateTime LastLoginDate { get; set; }
        public int NumberOfFailedAttempts { get; set; }
        public string Email { get; set; }
        public List<UserRole> Roles { get; set; }
        public IIdentity Identity { get; set; }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }
}
