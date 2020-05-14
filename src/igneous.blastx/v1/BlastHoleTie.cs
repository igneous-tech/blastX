using Newtonsoft.Json;
using System.Collections.Generic;

namespace igneous.blastx.v1
{
    public sealed class BlastHoleTie
    {
        [JsonProperty("startHoleId", Required = Required.Default)]
        public string StartHoleId { get; set; } = string.Empty;

        [JsonProperty("endHoleId", Required = Required.Always)]
        public string EndHoleId { get; set; } = string.Empty;

        [JsonProperty("blastProductId", Required = Required.Always)]
        public string BlastProductId { get; set; } = string.Empty;

        public override bool Equals(object obj) =>
            obj is BlastHoleTie tie &&
            StartHoleId == tie.StartHoleId &&
            EndHoleId == tie.EndHoleId &&
            BlastProductId == tie.BlastProductId;

        public override int GetHashCode()
        {
            var hashCode = -357853834;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(StartHoleId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EndHoleId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(BlastProductId);
            return hashCode;
        }
    }
}
