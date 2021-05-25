using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace RodnaPamet.Models
{
    [KnownType(typeof(User))]
    [Serializable, DataContract]
    public class User : RestItem
    {
        [DataMember]
        public string EMail { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public DateTime Registered { get; set; }
        [DataMember]
        public DateTime LastLogin { get; set; }
        [DataMember]
        public int AccessLevel { get; set; }
        [DataMember]
        public string EMailVerified { get; set; }
        [DataMember]
        public string VerificationCode { get; set; }
    }
}
