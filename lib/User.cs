using CoreBusiness.Common;
using CoreBusiness.Contracts;
using NPoco;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CoreBusiness.Data
{
    [Serializable]
    [DataContract]
    [PrimaryKey("ID", AutoIncrement =true)]
    public class User : BusinessObject
    {
        [DataMember]
        public virtual string Username { get; set; }
        [DataMember]
        public virtual string Email { get; set; }
        [DataMember]
        public virtual string Pin { get; set; }
        [DataMember]
        public virtual long MerchantID { get; set; }
        [DataMember]
        public virtual string MerchantName { get; set; }
        [DataMember]
        public virtual DateTime LastAuthenticated { get; set; }
        [DataMember]
        public virtual bool IsLockedOut { get; set; }
        [DataMember]
        public virtual long MasterID { get; set; }
        [DataMember]
        public virtual string Password { get; set; }
        [DataMember]
        public virtual string PassPhrase { get; set; }
        [DataMember]
        public virtual string PhoneNumber { get; set; }
        [DataMember]
        public virtual string FirstName { get; set; }
        [DataMember]
        public virtual string LastName { get; set; }
        [DataMember]
        public virtual string Address { get; set; }
        [DataMember]
        public virtual string Avatar { get; set; }
        [DataMember]
        public virtual DateTime? LastLoginDate { get; set; }
        [DataMember]
        public virtual int NumberOfFailedAttempts { get; set; }
        [DataMember]
        public virtual string EncryptionKey { get; set; }
        [DataMember]
        public virtual string DecryptionKey { get; set; }
        [DataMember]
        public virtual DateTime? DOB { get; set; }
        [DataMember]
        public virtual string[,] SecurityGrid { get; set; }
    }
    [Serializable]
    public class UserMap : BusinessObjectMap<User>
    {
    }
}
