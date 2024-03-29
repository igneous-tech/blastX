﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace igneous.blastx.v1
{
    public sealed class Layer
    {
        /// <summary>Distance from the top of the hole to the top of the layer. Measured in m</summary>
        [JsonProperty("topDepth", Required = Required.Always)]
        public double TopDepth { get; set; }

        /// <summary>Height or length of this layer. Measured in m</summary>
        [JsonProperty("height", Required = Required.Always)]
        public double Height { get; set; }

        /// <summary>Weight of product loaded in this layer. Measured in kg"</summary>
        [JsonProperty("weight", Required = Required.Default)]
        public double? Weight { get; set; }

        /// <summary>Specifies the optional loaded density of the product if it differs from the product 'at rest'. Measured in g/cc"</summary>
        [JsonProperty("loadedDensity", Required = Required.Default)]
        public double? LoadedDensity { get; set; }

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

        [JsonProperty("state", Required = Required.Default)]
        public string State { get; set; }

        [JsonProperty("notes", Required = Required.Default)]
        public string Notes { get; set; }

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
            DualDelayConnectionType == layer.DualDelayConnectionType &&
            State == layer.State &&
            Notes == layer.Notes;

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
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(State);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Notes);
            return hashCode;
        }
    }
}
