using System;
using System.Runtime.Serialization;

namespace RodnaPamet.Models
{
    [Serializable, DataContract, KnownTypeAttribute(typeof(Item))]
    public class Item : RestItem
    {
        [DataMember]
        public DateTime Created { get; set; }
        [DataMember]
        public string Subject { get; set; }
        [DataMember]
        public int Age { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string SubDescription { get; set; }
        [DataMember]
        public string Cameraman { get; set; }
        [DataMember]
        public string Filename { get; set; }
        [DataMember]
        public string Village { get; set; }
        [DataMember]
        public bool Uploaded { get; set; }
        [DataMember]
        public int Type { get; set; } // 0: values, 1: story
        [DataMember]
        public string TypeDescription { get; set; }
        public bool Uploading { get; set; } = false;
        [DataMember]
        public bool InfoComplete { get; set; } = false;
    }
}