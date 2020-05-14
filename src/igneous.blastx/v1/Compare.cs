using System.Collections.Generic;
using System.Linq;

namespace igneous.blastx.v1
{
    static class Compare
    {
        public static bool IsEquivalentTo<T>(this IList<T> value1, IList<T> value2)
        {
            if (value1 == value2 ||
                (value1 == null && value2.Count == 0) ||
                (value1.Count == 0 && value2 == null))
                return true;

            return Enumerable.SequenceEqual(value1, value2);
        }
    }
}
