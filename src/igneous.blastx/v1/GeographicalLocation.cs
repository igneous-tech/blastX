using Newtonsoft.Json;

namespace igneous.blastx.v1
{
    /// <summary>A geographical coordinate on the Earth</summary>
    public sealed class GeographicalLocation
    {
        [JsonProperty("latitude", Required = Required.Always)]
        public double Latitude { get; set; }

        [JsonProperty("longitude", Required = Required.Always)]
        public double Longitude { get; set; }

        [JsonProperty("elevation", Required = Required.Default)]
        public double? Elevation { get; set; }

        public override bool Equals(object obj) =>
            obj is GeographicalLocation location &&
            Latitude == location.Latitude &&
            Longitude == location.Longitude &&
            Elevation == location.Elevation;

        public override int GetHashCode()
        {
            var hashCode = 1960202551;
            hashCode = hashCode * -1521134295 + Latitude.GetHashCode();
            hashCode = hashCode * -1521134295 + Longitude.GetHashCode();
            hashCode = hashCode * -1521134295 + Elevation.GetHashCode();
            return hashCode;
        }
    }
}
