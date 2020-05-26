using Newtonsoft.Json;
using System.Collections.Generic;

namespace igneous.blastx.v1
{
    public partial class BlastHole
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("displayName", Required = Required.Default)]
        public string DisplayName { get; set; }

        [JsonProperty("holeNumber", Required = Required.Default)]
        public int? HoleNumber { get; set; }

        /// <summary>Measured in m. NOTE: hole depth may be implicitly described by the depthSamples array if present.</summary>
        [JsonProperty("depth", Required = Required.Default)]
        public double? Depth { get; set; }

        /// <summary>Measured in m</summary>
        [JsonProperty("subdrill", Required = Required.Default)]
        public double? Subdrill { get; set; }

        /// <summary>Measured in mm</summary>
        [JsonProperty("diameter", Required = Required.Default)]
        public double? Diameter { get; set; }

        /// <summary>Measured in degrees</summary>
        [JsonProperty("drillingAngle", Required = Required.Default)]
        public double? DrillingAngle { get; set; }

        /// <summary>Applicable to angled holes only. Measured in degrees. North is 0°, East is 90°, South is 180°, West is 270°.</summary>
        [JsonProperty("drillingBearing", Required = Required.Default)]
        public double? DrillingBearing { get; set; }

        /// <summary>This optional property will contain an array of offset sample data to describe the borehole deviation if available.</summary>
        [JsonProperty("depthSamples", Required = Required.Default)]
        public List<DepthSample> DepthSamples { get; set; } = new List<DepthSample>();

        /// <summary>Left offset of the center of the hole relative to the canvas. Measured in m</summary>
        [JsonProperty("centerLeftOffset", Required = Required.Default)]
        public double? CenterLeftOffset { get; set; }

        /// <summary>Top offset of the center of the hole relative to the canvas. Measured in m</summary>
        [JsonProperty("centerTopOffset", Required = Required.Default)]
        public double? CenterTopOffset { get; set; }

        [JsonProperty("patternId", Required = Required.Default)]
        public string PatternId { get; set; }

        [JsonProperty("holeLoadId", Required = Required.Default)]
        public string HoleLoadId { get; set; }

        [JsonProperty("location", Required = Required.Default)]
        public GeographicalLocation Location { get; set; }

        [JsonProperty("extensionData", Required = Required.Default)]
        public List<object> ExtensionData { get; set; } = new List<object>();

        public override bool Equals(object obj) =>
            obj is BlastHole hole &&
            Id == hole.Id &&
            DisplayName == hole.DisplayName &&
            HoleNumber == hole.HoleNumber &&
            Depth == hole.Depth &&
            Subdrill == hole.Subdrill &&
            Diameter == hole.Diameter &&
            DrillingAngle == hole.DrillingAngle &&
            DrillingBearing == hole.DrillingBearing &&
            DepthSamples.IsEquivalentTo(hole.DepthSamples) &&
            CenterLeftOffset == hole.CenterLeftOffset &&
            CenterTopOffset == hole.CenterTopOffset &&
            PatternId == hole.PatternId &&
            HoleLoadId == hole.HoleLoadId &&
            Equals(Location, hole.Location) &&
            ExtensionData.IsEquivalentTo(hole.ExtensionData);

        public override int GetHashCode()
        {
            var hashCode = 77282116;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DisplayName);
            hashCode = hashCode * -1521134295 + HoleNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + Depth.GetHashCode();
            hashCode = hashCode * -1521134295 + Subdrill.GetHashCode();
            hashCode = hashCode * -1521134295 + Diameter.GetHashCode();
            hashCode = hashCode * -1521134295 + DrillingAngle.GetHashCode();
            hashCode = hashCode * -1521134295 + DrillingBearing.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<DepthSample>>.Default.GetHashCode(DepthSamples);
            hashCode = hashCode * -1521134295 + CenterLeftOffset.GetHashCode();
            hashCode = hashCode * -1521134295 + CenterTopOffset.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PatternId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(HoleLoadId);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<object>>.Default.GetHashCode(ExtensionData);
            return hashCode;
        }
    }
}
