using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace igneous.blastx.v1
{
    public sealed class BlastInfo
    {
        [JsonProperty("blastDateTime", Required = Required.Default)]
        public string BlastDateTime { get; set; }

        /// <summary>System of measurement</summary>
        [JsonProperty("customer", Required = Required.Default)]
        public string Customer { get; set; }

        [JsonProperty("initiationType", Required = Required.Default)]
        [JsonConverter(typeof(StringEnumConverter))]
        public InitiationType? InitiationType { get; set; }

        /// <summary>Measured in degrees. North is 0°, East is 90°, South is 180°, West is 270°.</summary>
        [JsonProperty("bearing", Required = Required.Default)]
        public double? Bearing { get; set; }

        /// <summary>Number of diagram pixels in one unit of length</summary>
        [JsonProperty("diagramPixelScale", Required = Required.Default)]
        public double? DiagramPixelScale { get; set; }

        public override bool Equals(object obj) =>
            obj is BlastInfo info &&
            BlastDateTime == info.BlastDateTime &&
            Customer == info.Customer &&
            InitiationType == info.InitiationType &&
            Bearing == info.Bearing &&
            DiagramPixelScale == info.DiagramPixelScale;

        public override int GetHashCode()
        {
            var hashCode = 492832657;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(BlastDateTime);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Customer);
            hashCode = hashCode * -1521134295 + InitiationType.GetHashCode();
            hashCode = hashCode * -1521134295 + Bearing.GetHashCode();
            hashCode = hashCode * -1521134295 + DiagramPixelScale.GetHashCode();
            return hashCode;
        }
    }
}
