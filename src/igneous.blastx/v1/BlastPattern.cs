using Newtonsoft.Json;
using System.Collections.Generic;

namespace igneous.blastx.v1
{
    public sealed class BlastPattern
    {
        [JsonProperty("id", Required = Required.Default)]
        public string Id { get; set; }

        [JsonProperty("displayName", Required = Required.Default)]
        public string DisplayName { get; set; }

        /// <summary>Measured in m</summary>
        [JsonProperty("burden", Required = Required.Default)]
        public double? Burden { get; set; }

        /// <summary>Measured in m</summary>
        [JsonProperty("spacing", Required = Required.Default)]
        public double? Spacing { get; set; }

        [JsonProperty("numberOfHoles", Required = Required.Always)]
        public int NumberOfHoles { get; set; }

        [JsonProperty("numberOfRows", Required = Required.Default)]
        public int? NumberOfRows { get; set; }

        [JsonProperty("numberOfColumns", Required = Required.Default)]
        public int? NumberOfColumns { get; set; }

        /// <summary>Describes geometrical rotation of the pattern relative to its top left corner. Positive angles are clockwise. Measured in degrees.</summary>
        [JsonProperty("rotationAngle", Required = Required.Default)]
        public double? RotationAngle { get; set; }

        /// <summary>Left offset of the top left corner relative to the canvas. Measured in m</summary>
        [JsonProperty("leftOffset", Required = Required.Default)]
        public double? LeftOffset { get; set; }

        /// <summary>Top offset of the top left corner relative to the canvas. Measured in m</summary>
        [JsonProperty("topOffset", Required = Required.Default)]
        public double? TopOffset { get; set; }

        [JsonProperty("extensionData", Required = Required.Default)]
        public List<object> ExtensionData { get; set; } = new List<object>();

        public override bool Equals(object obj) =>
            obj is BlastPattern pattern &&
            Id == pattern.Id &&
            DisplayName == pattern.DisplayName &&
            Burden == pattern.Burden &&
            Spacing == pattern.Spacing &&
            NumberOfHoles == pattern.NumberOfHoles &&
            NumberOfRows == pattern.NumberOfRows &&
            NumberOfColumns == pattern.NumberOfColumns &&
            RotationAngle == pattern.RotationAngle &&
            LeftOffset == pattern.LeftOffset &&
            TopOffset == pattern.TopOffset &&
            ExtensionData.IsEquivalentTo(pattern.ExtensionData);

        public override int GetHashCode()
        {
            var hashCode = 1645832953;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DisplayName);
            hashCode = hashCode * -1521134295 + Burden.GetHashCode();
            hashCode = hashCode * -1521134295 + Spacing.GetHashCode();
            hashCode = hashCode * -1521134295 + NumberOfHoles.GetHashCode();
            hashCode = hashCode * -1521134295 + NumberOfRows.GetHashCode();
            hashCode = hashCode * -1521134295 + NumberOfColumns.GetHashCode();
            hashCode = hashCode * -1521134295 + RotationAngle.GetHashCode();
            hashCode = hashCode * -1521134295 + LeftOffset.GetHashCode();
            hashCode = hashCode * -1521134295 + TopOffset.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<object>>.Default.GetHashCode(ExtensionData);
            return hashCode;
        }
    }
}
