using System.Collections.Generic;

namespace igneous.blastx.v1
{
    public interface IHasExtensionData
    {
        List<object> ExtensionData { get; set; }
    }

    public static class HasExtensionDataExtensions
    {
        public static List<object> GetExtensionData(this IHasExtensionData obj) =>
            obj.ExtensionData ??
            (obj.ExtensionData = new List<object>());
    }
}
