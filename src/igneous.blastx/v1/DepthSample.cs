using Newtonsoft.Json;

namespace igneous.blastx.v1
{
    public sealed class DepthSample
    {
        /// <summary>The offset in the easting direction as measured from the collar. Measured in m.</summary>
        [JsonProperty("eastingOffset", Required = Required.Always)]
        public double EastingOffset { get; set; }

        /// <summary>The offset in the northing direction as measured from the collar. Measured in m.</summary>
        [JsonProperty("northingOffset", Required = Required.Always)]
        public double NorthingOffset { get; set; }

        /// <summary>The vertical elevation down from the collar for this sample. Measured in m.</summary>
        [JsonProperty("depth", Required = Required.Always)]
        public double Depth { get; set; }

        public override bool Equals(object obj) =>
            obj is DepthSample sample &&
            EastingOffset == sample.EastingOffset &&
            NorthingOffset == sample.NorthingOffset &&
            Depth == sample.Depth;

        public override int GetHashCode()
        {
            var hashCode = 737823080;
            hashCode = hashCode * -1521134295 + EastingOffset.GetHashCode();
            hashCode = hashCode * -1521134295 + NorthingOffset.GetHashCode();
            hashCode = hashCode * -1521134295 + Depth.GetHashCode();
            return hashCode;
        }
    }
}
