using Newtonsoft.Json;
using System.Collections.Generic;

namespace igneous.blastx.v1
{
    public sealed class Cost : IHasExtensionData
    {
        [JsonProperty("amount", Required = Required.Always)]
        public double Amount { get; set; }

        [JsonProperty("currency", Required = Required.Default)]
        public double Currency { get; set; }

        [JsonProperty("extensionData", Required = Required.Default)]
        public List<object> ExtensionData { get; set; }

        [JsonProperty("notes", Required = Required.Default)]
        public string Notes { get; set; }

        public override bool Equals(object obj) =>
            obj is Cost cost &&
            Amount == cost.Amount &&
            Currency == cost.Currency &&
            ExtensionData.IsEquivalentTo(cost.ExtensionData) &&
            Equals(Notes, cost.Notes);

        public override int GetHashCode()
        {
            int hashCode = 1798114747;
            hashCode = hashCode * -1521134295 + Amount.GetHashCode();
            hashCode = hashCode * -1521134295 + Currency.GetHashCode();
            if (ExtensionData != null)
                foreach (var item in ExtensionData)
                    hashCode *= -1521134295 + EqualityComparer<object>.Default.GetHashCode(item);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Notes);
            return hashCode;
        }
    }
}
