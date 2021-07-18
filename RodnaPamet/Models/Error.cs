using System;
using System.Runtime.Serialization;

namespace RodnaPamet.Models
{
    [KnownType(typeof(Error))]
    [Serializable, DataContract]
    public class Error : RestItem
    {
        [DataMember]
        public string ErrorMessage { get; set; }
        [DataMember]
        public string Device { get; set; }
        [DataMember]
        public string UserId { get; set; }
    }
}
