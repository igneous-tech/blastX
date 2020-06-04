using Newtonsoft.Json;
using System.Collections.Generic;

namespace igneous.blastx.v1
{
    public sealed class ElectronicDetonator : IHasExtensionData
    {
        [JsonProperty("blastHoleId", Required = Required.Always)]
        public string BlastHoleId { get; set; } = string.Empty;

        [JsonProperty("detNumber", Required = Required.Always)]
        public int DetNumber { get; set; }

        [JsonProperty("branch", Required = Required.Default)]
        public string Branch { get; set; }

        [JsonProperty("deckNumber", Required = Required.Default)]
        public int? DeckNumber { get; set; }

        /// <summary>Delay time in milliseconds</summary>
        [JsonProperty("delayTime", Required = Required.Default)]
        public double? DelayTime { get; set; }

        [JsonProperty("serialNumber", Required = Required.Default)]
        public string SerialNumber { get; set; }

        /// <summary>Distance from the top of the hole to the detonator. Measured in m.</summary>
        [JsonProperty("depth", Required = Required.Default)]
        public double? Depth { get; set; }

        [JsonProperty("extensionData", Required = Required.Default)]
        public List<object> ExtensionData { get; set; }

        [JsonProperty("cost", Required = Required.Default)]
        public Cost Cost { get; set; }

        public override bool Equals(object obj) =>
            obj is ElectronicDetonator detonator &&
            BlastHoleId == detonator.BlastHoleId &&
            DetNumber == detonator.DetNumber &&
            Branch == detonator.Branch &&
            DeckNumber == detonator.DeckNumber &&
            DelayTime == detonator.DelayTime &&
            SerialNumber == detonator.SerialNumber &&
            Depth == detonator.Depth &&
            ExtensionData.IsEquivalentTo(detonator.ExtensionData) &&
            Equals(Cost, detonator.Cost);

        public override int GetHashCode()
        {
            int hashCode = 641344168;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(BlastHoleId);
            hashCode = hashCode * -1521134295 + DetNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Branch);
            hashCode = hashCode * -1521134295 + DeckNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + DelayTime.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SerialNumber);
            hashCode = hashCode * -1521134295 + Depth.GetHashCode();
            if (ExtensionData != null)
                foreach (var item in ExtensionData)
                    hashCode *= -1521134295 + Compare.GetHashCode(item);
            hashCode = hashCode * -1521134295 + EqualityComparer<Cost>.Default.GetHashCode(Cost);
            return hashCode;
        }
    }
}
