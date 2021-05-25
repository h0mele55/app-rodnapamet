using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace RodnaPamet.Models
{
    [Serializable, DataContract]
    class ServerResponse
    {
        [DataMember]
        [JsonProperty("Success")]
        public int Success { get; set; }
        [DataMember]
        [JsonProperty("RecordId")]
        public Guid RecordId { get; set; }
        [DataMember]
        [JsonProperty("Error")]
        public int Error { get; set; }

        [DataMember]
        [JsonProperty("Message")]
        public string Message { get; set; }
        [DataMember]
        [JsonProperty("Data")]
        public object Data { get; set; }

        public static ServerResponse FromJson(string json) => JsonConvert.DeserializeObject<ServerResponse>(json, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
