using Newtonsoft.Json;
using System.Collections.Generic;

namespace igneous.blastx.v1
{
    public sealed class BlastHoleTie : IHasExtensionData
    {
        [JsonProperty("startHoleId", Required = Required.Default)]
        public string StartHoleId { get; set; } = string.Empty;

        [JsonProperty("endHoleId", Required = Required.Always)]
        public string EndHoleId { get; set; } = string.Empty;

        [JsonProperty("blastProductId", Required = Required.Always)]
        public string BlastProductId { get; set; } = string.Empty;

        [JsonProperty("extensionData", Required = Required.Default)]
        public List<object> ExtensionData { get; set; }

        [JsonProperty("cost", Required = Required.Default)]
        public Cost Cost { get; set; }

        public override bool Equals(object obj) =>
            obj is BlastHoleTie tie &&
            StartHoleId == tie.StartHoleId &&
            EndHoleId == tie.EndHoleId &&
            BlastProductId == tie.BlastProductId &&
            ExtensionData.IsEquivalentTo(tie.ExtensionData) &&
            Equals(Cost, tie.Cost);

        public override int GetHashCode()
        {
            var hashCode = -357853834;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(StartHoleId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EndHoleId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(BlastProductId);
            if (ExtensionData != null)
                foreach (var item in ExtensionData)
                    hashCode *= -1521134295 + Compare.GetHashCode(item);
            hashCode = hashCode * -1521134295 + EqualityComparer<Cost>.Default.GetHashCode(Cost);
            return hashCode;
        }
    }
}
