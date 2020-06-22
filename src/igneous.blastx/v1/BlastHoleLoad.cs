using Newtonsoft.Json;
using System.Collections.Generic;

namespace igneous.blastx.v1
{
    public sealed class BlastHoleLoad : IHasExtensionData
    {
        [JsonProperty("id", Required = Required.Default)]
        public string Id { get; set; }

        [JsonProperty("displayName", Required = Required.Default)]
        public string DisplayName { get; set; }

        [JsonProperty("decks", Required = Required.Default)]
        public List<Deck> Decks { get; set; } = new List<Deck>();

        [JsonProperty("notes", Required = Required.Default)]
        public string Notes { get; set; }

        [JsonProperty("extensionData", Required = Required.Default)]
        public List<object> ExtensionData { get; set; }

        public override bool Equals(object obj) =>
            obj is BlastHoleLoad load &&
            Id == load.Id &&
            DisplayName == load.DisplayName &&
            Decks.IsEquivalentTo(load.Decks) &&
            Notes == load.Notes &&
            ExtensionData.IsEquivalentTo(load.ExtensionData);

        public override int GetHashCode()
        {
            var hashCode = -130742393;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DisplayName);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Deck>>.Default.GetHashCode(Decks);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Notes);
            if (ExtensionData != null)
                foreach (var item in ExtensionData)
                    hashCode *= -1521134295 + Compare.GetHashCode(item);
            return hashCode;
        }
    }
}
