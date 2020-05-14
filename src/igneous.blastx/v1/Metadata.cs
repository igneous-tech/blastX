using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace igneous.blastx.v1
{
    public sealed class Metadata
    {
        [JsonProperty("id", Required = Required.Default)]
        public string Id { get; set; }

        [JsonProperty("displayName", Required = Required.Default)]
        public string DisplayName { get; set; }

        /// <summary>Application in which this blast originated</summary>
        [JsonProperty("dataSource", Required = Required.Always)]
        public string DataSource { get; set; } = "<undefined>";

        /// <summary>Standard used to define geographical coordinate on the Earth</summary>
        [JsonProperty("geodeticStandard", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public GeodeticStandard GeodeticStandard { get; set; } = GeodeticStandard.WGS1984;

        /// <summary>Date and time (in ISO 8601) format of when this document was generated</summary>
        [JsonProperty("createdDateTime", Required = Required.Always)]
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        public string ToJson() => JsonConvert.SerializeObject(this);

        public static Metadata FromJson(string data) =>
            JsonConvert.DeserializeObject<Metadata>(data);

        public override bool Equals(object obj) =>
            obj is Metadata metadata &&
            Id == metadata.Id &&
            DisplayName == metadata.DisplayName &&
            DataSource == metadata.DataSource &&
            GeodeticStandard == metadata.GeodeticStandard &&
            CreatedDateTime == metadata.CreatedDateTime;

        public override int GetHashCode()
        {
            var hashCode = 376710594;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DisplayName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DataSource);
            hashCode = hashCode * -1521134295 + GeodeticStandard.GetHashCode();
            hashCode = hashCode * -1521134295 + CreatedDateTime.GetHashCode();
            return hashCode;
        }
    }
}
