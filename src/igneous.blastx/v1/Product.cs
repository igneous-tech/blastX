using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace igneous.blastx.v1
{
    public sealed class Product : IHasExtensionData
    {
        [JsonProperty("id", Required = Required.Default)]
        public string Id { get; set; }

        [JsonProperty("displayName", Required = Required.Always)]
        public string DisplayName { get; set; } = string.Empty;

        [JsonProperty("productCode", Required = Required.Default)]
        public string ProductCode { get; set; }

        [JsonProperty("productType", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public ProductType ProductType { get; set; } = ProductType.Bulk;

        [JsonProperty("detonatorType", Required = Required.Default)]
        [JsonConverter(typeof(StringEnumConverter))]
        public DetonatorType? DetonatorType { get; set; }

        [JsonProperty("delayType", Required = Required.Default)]
        [JsonConverter(typeof(StringEnumConverter))]
        public DelayType? DelayType { get; set; }

        /// <summary>Measured in g/cm3</summary>
        [JsonProperty("density", Required = Required.Default)]
        public double? Density { get; set; }

        /// <summary>Measured in m/s</summary>
        [JsonProperty("detonationVelocity", Required = Required.Default)]
        public double? DetonationVelocity { get; set; }

        /// <summary>Measured in m</summary>
        [JsonProperty("length", Required = Required.Default)]
        public double? Length { get; set; }

        /// <summary>Measured in mm</summary>
        [JsonProperty("diameter", Required = Required.Default)]
        public double? Diameter { get; set; }

        /// <summary>Measured in kg</summary>
        [JsonProperty("weight", Required = Required.Default)]
        public double? Weight { get; set; }

        /// <summary>Delay time in milliseconds</summary>
        [JsonProperty("inHoleDelayTime", Required = Required.Default)]
        public double? InHoleDelayTime { get; set; }

        /// <summary>Delay time in milliseconds</summary>
        [JsonProperty("surfaceDelayTime", Required = Required.Default)]
        public double? SurfaceDelayTime { get; set; }

        [JsonProperty("notes", Required = Required.Default)]
        public string Notes { get; set; }

        [JsonProperty("extensionData", Required = Required.Default)]
        public List<object> ExtensionData { get; set; }

        [JsonProperty("cost", Required = Required.Default)]
        public Cost Cost { get; set; }

        public override bool Equals(object obj) =>
            obj is Product product &&
            Equals(Id, product.Id) &&
            ProductCode == product.ProductCode &&
            ProductType == product.ProductType &&
            DetonatorType == product.DetonatorType &&
            DelayType == product.DelayType &&
            Density == product.Density &&
            DetonationVelocity == product.DetonationVelocity &&
            Length == product.Length &&
            Diameter == product.Diameter &&
            Weight == product.Weight &&
            InHoleDelayTime == product.InHoleDelayTime &&
            SurfaceDelayTime == product.SurfaceDelayTime &&
            Notes == product.Notes &&
            ExtensionData.IsEquivalentTo(product.ExtensionData) &&
            Equals(Cost, product.Cost);

        public override int GetHashCode()
        {
            var hashCode = 770216122;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ProductCode);
            hashCode = hashCode * -1521134295 + ProductType.GetHashCode();
            hashCode = hashCode * -1521134295 + DetonatorType.GetHashCode();
            hashCode = hashCode * -1521134295 + DelayType.GetHashCode();
            hashCode = hashCode * -1521134295 + Density.GetHashCode();
            hashCode = hashCode * -1521134295 + DetonationVelocity.GetHashCode();
            hashCode = hashCode * -1521134295 + Length.GetHashCode();
            hashCode = hashCode * -1521134295 + Diameter.GetHashCode();
            hashCode = hashCode * -1521134295 + Weight.GetHashCode();
            hashCode = hashCode * -1521134295 + InHoleDelayTime.GetHashCode();
            hashCode = hashCode * -1521134295 + SurfaceDelayTime.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Notes);
            if (ExtensionData != null)
                foreach (var item in ExtensionData)
                    hashCode *= -1521134295 + Compare.GetHashCode(item);
            hashCode = hashCode * -1521134295 + EqualityComparer<Cost>.Default.GetHashCode(Cost);
            return hashCode;
        }
    }
}
