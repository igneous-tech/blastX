using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace igneous.blastx.v1
{
    public sealed class Deck
    {
        [JsonProperty("deckNumber", Required = Required.Always)]
        public int DeckNumber { get; set; }

        [JsonProperty("layers", Required = Required.Always)]
        public List<Layer> Layers { get; set; } = new List<Layer>();

        public override bool Equals(object obj) =>
            obj is Deck deck &&
            DeckNumber == deck.DeckNumber &&
            Layers.IsEquivalentTo(deck.Layers);

        public override int GetHashCode()
        {
            var hashCode = 153008064;
            hashCode = hashCode * -1521134295 + DeckNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Layer>>.Default.GetHashCode(Layers);
            return hashCode;
        }
    }
}
