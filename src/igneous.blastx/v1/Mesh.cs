using Newtonsoft.Json;
using System.Collections.Generic;

namespace igneous.blastx.v1
{
    public sealed class Mesh : IHasExtensionData
    {
        [JsonProperty("id", Required = Required.Default)]
        public string Id { get; set; }

        [JsonProperty("displayName", Required = Required.Default)]
        public string DisplayName { get; set; }

        [JsonProperty("points", Required = Required.Always)]
        public List<LinearCoordinate> Points { get; set; } = new List<LinearCoordinate>();

        [JsonProperty("indices", Required = Required.Always)]
        public List<int> Indices { get; set; } = new List<int>();

        [JsonProperty("textureCoordinates", Required = Required.Default)]
        public List<TextureCoordinate> TextureCoordinates { get; set; } = new List<TextureCoordinate>();

        [JsonProperty("texture", Required = Required.Default)]
        public string Texture { get; set; }

        [JsonProperty("extensionData", Required = Required.Default)]
        public List<object> ExtensionData { get; set; }

        public override bool Equals(object obj) =>
            obj is Mesh mesh &&
            Id == mesh.Id &&
            DisplayName == mesh.DisplayName &&
            Points.IsEquivalentTo(mesh.Points) &&
            Indices.IsEquivalentTo(mesh.Indices) &&
            TextureCoordinates.IsEquivalentTo(mesh.TextureCoordinates) &&
            Texture == mesh.Texture;

        public override int GetHashCode()
        {
            var hashCode = -1640476891;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DisplayName);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<LinearCoordinate>>.Default.GetHashCode(Points);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<int>>.Default.GetHashCode(Indices);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<TextureCoordinate>>.Default.GetHashCode(TextureCoordinates);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Texture);
            if (ExtensionData != null)
                foreach (var item in ExtensionData)
                    hashCode *= -1521134295 + EqualityComparer<object>.Default.GetHashCode(item);
            return hashCode;
        }
    }
}
