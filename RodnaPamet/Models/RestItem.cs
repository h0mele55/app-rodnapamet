﻿using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace RodnaPamet.Models
{
    [Serializable, DataContract]
    public class RestItem
    {
        [DataMember]
        [JsonProperty("Success")]
        public string Id { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
