using Newtonsoft.Json;

namespace igneous.blastx.v1
{
    public sealed class TextureCoordinate
    {
        [JsonProperty("U", Required = Required.Always)]
        public double U { get; set; }

        [JsonProperty("V", Required = Required.Always)]
        public double V { get; set; }

        public override bool Equals(object obj) =>
            obj is TextureCoordinate coordinate &&
            U == coordinate.U &&
            V == coordinate.V;

        public override int GetHashCode()
        {
            var hashCode = 1138419364;
            hashCode = hashCode * -1521134295 + U.GetHashCode();
            hashCode = hashCode * -1521134295 + V.GetHashCode();
            return hashCode;
        }
    }
}
