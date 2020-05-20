using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace igneous.blastx.v1
{
    public sealed class Layer
    {
        /// <summary>Distance from the top of the hole to the top of the layer. Measured in m.</summary>
        [JsonProperty("topDepth", Required = Required.Always)]
        public double TopDepth { get; set; }

        /// <summary>Measured in m</summary>
        [JsonProperty("height", Required = Required.Default)]
        public double? Height { get; set; }

        [JsonProperty("layerType", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public LayerType LayerType { get; set; }

        [JsonProperty("stemmingDescription", Required = Required.Default)]
        public string StemmingDescription { get; set; }

        [JsonProperty("blastProductId", Required = Required.Default)]
        public string BlastProductId { get; set; }

        [JsonProperty("isSubstitutedAsBooster", Required = Required.Default)]
        public bool? IsSubstitutedAsBooster { get; set; }

        /// <summary>Delay time in milliseconds</summary>
        [JsonProperty("delayTime", Required = Required.Default)]
        public double? DelayTime { get; set; }

        [JsonProperty("detonatingCordConnectionType", Required = Required.Default)]
        [JsonConverter(typeof(StringEnumConverter))]
        public DetonatingCordConnectionType? DetonatingCordConnectionType { get; set; }

        [JsonProperty("dualDelayConnectionType", Required = Required.Default)]
        [JsonConverter(typeof(StringEnumConverter))]
        public DualDelayConnectionType? DualDelayConnectionType { get; set; }

        public override bool Equals(object obj) =>
            obj is Layer layer &&
            TopDepth == layer.TopDepth &&
            Height == layer.Height &&
            LayerType == layer.LayerType &&
            StemmingDescription == layer.StemmingDescription &&
            BlastProductId == layer.BlastProductId &&
            IsSubstitutedAsBooster == layer.IsSubstitutedAsBooster &&
            DelayTime == layer.DelayTime &&
            DetonatingCordConnectionType == layer.DetonatingCordConnectionType &&
            DualDelayConnectionType == layer.DualDelayConnectionType;

        public override int GetHashCode()
        {
            var hashCode = -237798683;
            hashCode = hashCode * -1521134295 + TopDepth.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            hashCode = hashCode * -1521134295 + LayerType.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(StemmingDescription);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(BlastProductId);
            hashCode = hashCode * -1521134295 + IsSubstitutedAsBooster.GetHashCode();
            hashCode = hashCode * -1521134295 + DelayTime.GetHashCode();
            hashCode = hashCode * -1521134295 + DetonatingCordConnectionType.GetHashCode();
            hashCode = hashCode * -1521134295 + DualDelayConnectionType.GetHashCode();
            return hashCode;
        }
    }
}
