using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace igneous.blastx.v1
{
    /// <summary>A blast performed using explosives to break rock for excavation</summary>
    public sealed class Blast : IHasExtensionData
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

        [JsonProperty("notes", Required = Required.Default)]
        public string Notes { get; set; }

        [JsonProperty("extensionData", Required = Required.Default)]
        public List<object> ExtensionData { get; set; }

        public static string ToJson(object obj) => JsonConvert.SerializeObject(
            obj,
            Formatting.Indented,
            new JsonSerializerSettings { ContractResolver = new SkipOptionalUndefinedResolver() });

        public string ToJson() => ToJson(this);

        public static Blast FromJson(string data) =>
            JsonConvert.DeserializeObject<Blast>(data);

        readonly Dictionary<Type, object> _cacheOfIdCaches =
            new Dictionary<Type, object>();

        public T Get<T>(string id)
            where T : class
        {
            Dictionary<string, T> idCacheAs;
            if (_cacheOfIdCaches.TryGetValue(typeof(T), out var idCache) == false)
            {
                _cacheOfIdCaches[typeof(T)] = idCacheAs = new Dictionary<string, T>();

                // Find the appropriate list of objects
                var itemProp = typeof(Blast).GetProperties()
                    .Where(p => p.PropertyType == typeof(List<T>))
                    .FirstOrDefault();
                var items = (List<T>)itemProp.GetValue(this);

                // Get the id property
                var idProp = typeof(T).GetProperty("Id");

                // Populate the cache with the appropriate data
                foreach (var item in items)
                    idCacheAs[(string)idProp.GetValue(item)] = item;
            }
            else
                idCacheAs = (Dictionary<string, T>)idCache;

            return idCacheAs.TryGetValue(id, out var value) ? value : null;
        }

        public IEnumerable<string> Validate()
        {
            // Make sure all items have unique id values
            var listProps = typeof(Blast).GetProperties()
                .Where(p => p.PropertyType.IsGenericType)
                .Where(p => p.PropertyType.GetGenericTypeDefinition() == typeof(List<>));
            foreach (var prop in listProps)
            {
                var idProp = prop.PropertyType.GenericTypeArguments[0].GetProperty("Id");
                var items = (IList)prop.GetValue(this);
                var idValues = new HashSet<string>();
                if (items != null)
                    foreach (var item in items)
                    {
                        var idValue = (string)idProp.GetValue(item);
                        if (string.IsNullOrWhiteSpace(idValue))
                            yield return $"Found empty id value on an object of type {item.GetType()}";
                        if (idValues.Add(idValue) == false)
                            yield return $"Duplicate id value found: {idValue}";
                    }
            }

            // Make sure all holes refer to a hole load that exists
            foreach (var hole in Holes)
                if (string.IsNullOrWhiteSpace(hole.HoleLoadId) == false)
                {
                    var load = Get<BlastHoleLoad>(hole.HoleLoadId);
                    if (load == null)
                        yield return $"{nameof(BlastHole)} with id {hole.Id} refers to load {hole.HoleLoadId} which doesn't exist";
                }

            // Make sure all layers refer to products that exist
            foreach (var load in HoleLoads)
                foreach (var deck in load.Decks)
                    foreach (var layer in deck.Layers)
                        if (string.IsNullOrWhiteSpace(layer.BlastProductId) == false)
                        {
                            if (Get<Product>(layer.BlastProductId) == null)
                                yield return $"{nameof(BlastHoleLoad)} with id {load.Id} refers to a product with id {layer.BlastProductId} which doesn't exist";
                        }

            // Make sure all layers refer to products that exist
            foreach (var tie in HoleTies)
                if (string.IsNullOrWhiteSpace(tie.BlastProductId) == false)
                {
                    if (Get<BlastHole>(tie.StartHoleId) == null)
                        yield return $"A {nameof(BlastHoleTie)} refers to a product with id {tie.StartHoleId} which doesn't exist";
                    if (Get<BlastHole>(tie.EndHoleId) == null)
                        yield return $"A {nameof(BlastHoleTie)} refers to a hole with id {tie.EndHoleId} which doesn't exist";
                    if (Get<Product>(tie.BlastProductId) == null)
                        yield return $"A {nameof(BlastHoleTie)} refers to a product with id {tie.BlastProductId} which doesn't exist";
                }

            // Validate the load data for each hole
            foreach (var hole in Holes)
                if (hole.GetLength() > 0)
                {
                    var load = Get<BlastHoleLoad>(hole.HoleLoadId);
                    if (load != null)
                    {
                        // TODO: ensure the load fits in the hole and that numbers are internally consistent
                    }
                }
        }

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
            Notes == blast.Notes &&
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
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Notes);
            if (ExtensionData != null)
                foreach (var item in ExtensionData)
                    hashCode *= -1521134295 + Compare.GetHashCode(item);
            return hashCode;
        }
    }
}
