using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Reflection;

namespace igneous.blastx.v1
{
    /// <summary>A blast performed using explosives to break rock for excavation</summary>
    public sealed class Blast
    {
        [JsonProperty("metadata", Required = Required.Always)]
        public Metadata Metadata { get; set; } = new Metadata();

        [JsonProperty("blastInfo", Required = Required.Default)]
        public BlastInfo BlastInfo { get; set; } = new BlastInfo();

        [JsonProperty("location", Required = Required.Default)]
        public GeographicalLocation Location { get; set; } = new GeographicalLocation();

        [JsonProperty("holeLoads", Required = Required.Default)]
        public List<BlastHoleLoad> HoleLoads { get; set; } = new List<BlastHoleLoad>();

        [JsonProperty("patterns", Required = Required.Default)]
        public List<BlastPattern> Patterns { get; set; } = new List<BlastPattern>();

        [JsonProperty("holes", Required = Required.Default)]
        public List<BlastHole> Holes { get; set; } = new List<BlastHole>();

        [JsonProperty("products", Required = Required.Default)]
        public List<Product> Products { get; set; } = new List<Product>();

        [JsonProperty("holeTies", Required = Required.Default)]
        public List<BlastHoleTie> HoleTies { get; set; } = new List<BlastHoleTie>();

        [JsonProperty("electronicDetonators", Required = Required.Default)]
        public List<ElectronicDetonator> ElectronicDetonators { get; set; } =
            new List<ElectronicDetonator>();

        [JsonProperty("meshes", Required = Required.Default)]
        public List<Mesh> Meshes { get; set; } = new List<Mesh>();

        [JsonProperty("extensionData", Required = Required.Default)]
        public List<object> ExtensionData { get; set; } = new List<object>();

        public static string ToJson(object obj) => JsonConvert.SerializeObject(
            obj,
            Formatting.Indented,
            new JsonSerializerSettings { ContractResolver = new SkipOptionalUndefinedResolver() });

        public string ToJson() => ToJson(this);

        public static Blast FromJson(string data) =>
            JsonConvert.DeserializeObject<Blast>(data);

        sealed class SkipOptionalUndefinedResolver : DefaultContractResolver
        {
            protected override JsonProperty CreateProperty(
                MemberInfo member, MemberSerialization memberSerialization)
            {
                var property = base.CreateProperty(member, memberSerialization);
                if (member.MemberType == MemberTypes.Property &&
                    property.Required == Required.Default)
                    property.ShouldSerialize =
                        p => Equals(p.GetType().GetProperty(member.Name).GetValue(p), null) == false;

                return property;
            }
        }

        public override bool Equals(object obj) =>
            obj is Blast blast &&
            Metadata.Equals(blast.Metadata) &&
            BlastInfo.Equals(blast.BlastInfo) &&
            Location.Equals(blast.Location) &&
            HoleLoads.IsEquivalentTo(blast.HoleLoads) &&
            Patterns.IsEquivalentTo(blast.Patterns) &&
            Holes.IsEquivalentTo(blast.Holes) &&
            Products.IsEquivalentTo(blast.Products) &&
            HoleTies.IsEquivalentTo(blast.HoleTies) &&
            ElectronicDetonators.IsEquivalentTo(blast.ElectronicDetonators) &&
            Meshes.IsEquivalentTo(blast.Meshes) &&
            ExtensionData.IsEquivalentTo(blast.ExtensionData);

        public override int GetHashCode()
        {
            var hashCode = 1737649936;
            hashCode = hashCode * -1521134295 + EqualityComparer<Metadata>.Default.GetHashCode(Metadata);
            hashCode = hashCode * -1521134295 + EqualityComparer<BlastInfo>.Default.GetHashCode(BlastInfo);
            hashCode = hashCode * -1521134295 + EqualityComparer<GeographicalLocation>.Default.GetHashCode(Location);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<BlastHoleLoad>>.Default.GetHashCode(HoleLoads);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<BlastPattern>>.Default.GetHashCode(Patterns);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<BlastHole>>.Default.GetHashCode(Holes);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Product>>.Default.GetHashCode(Products);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<BlastHoleTie>>.Default.GetHashCode(HoleTies);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<ElectronicDetonator>>.Default.GetHashCode(ElectronicDetonators);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Mesh>>.Default.GetHashCode(Meshes);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<object>>.Default.GetHashCode(ExtensionData);
            return hashCode;
        }
    }
}
