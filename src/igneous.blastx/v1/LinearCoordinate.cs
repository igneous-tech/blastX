using Newtonsoft.Json;

namespace igneous.blastx.v1
{
    public sealed class LinearCoordinate
    {
        [JsonProperty("E", Required = Required.Always)]
        public double E { get; set; }

        [JsonProperty("N", Required = Required.Always)]
        public double N { get; set; }

        [JsonProperty("L", Required = Required.Default)]
        public double? L { get; set; }

        public override bool Equals(object obj) =>
            obj is LinearCoordinate coordinate &&
            E == coordinate.E &&
            N == coordinate.N &&
            L == coordinate.L;

        public override int GetHashCode()
        {
            var hashCode = 1138419364;
            hashCode = hashCode * -1521134295 + E.GetHashCode();
            hashCode = hashCode * -1521134295 + N.GetHashCode();
            hashCode = hashCode * -1521134295 + L.GetHashCode();
            return hashCode;
        }
    }
}
