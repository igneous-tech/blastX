using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace igneous.blastx.v1
{
    public static class Compare
    {
        public static bool IsEquivalentTo<T>(this IList<T> value1, IList<T> value2)
        {
            if (value1 == value2 ||
                (value1 == null && value2.Count == 0) ||
                (value1.Count == 0 && value2 == null))
                return true;

            return Enumerable.SequenceEqual(value1, value2);
        }

        public static bool IsEquivalentTo(
            this ExpandoObject expando1, ExpandoObject expando2)
        {
            // If both are null, consider them equal
            if (expando1 == null &&
                expando2 == null)
                return true;

            var expandoDict1 = (IDictionary<string, object>)expando1;
            var expandoDict2 = (IDictionary<string, object>)expando2;

            if ((expando1 == null && expandoDict2.Count == 0) ||
                (expandoDict1.Count == 0 && expando2 == null) ||
                expandoDict1.Count != expandoDict2.Count)
                return false;

            foreach (var kv in expandoDict1)
                if (expandoDict2.TryGetValue(kv.Key, out var val2))
                    if (kv.Value is ExpandoObject expandoVal1 &&
                        val2 is ExpandoObject expandoVal2 &&
                        IsEquivalentTo(expandoVal1, expandoVal2) == false)
                        return false;
                    else if (Equals(kv.Value, val2) == false)
                        return false;

            return true;
        }

        public static bool IsEquivalentTo(
            this IList<ExpandoObject> value1, IList<ExpandoObject> value2)
        {
            if (value1 == value2 ||
                (value1 == null && value2.Count == 0) ||
                (value1.Count == 0 && value2 == null))
                return true;

            for (var n = 0; n < value1.Count; n++)
                if (IsEquivalentTo(value1[n], value2[n]) == false)
                    return false;

            return true;
        }

        public static int GetHashCode(this ExpandoObject expando)
        {
            if (expando == null)
                return 0;

            var hashCode = 1737649936;
            var expandoDict1 = (IDictionary<string, object>)expando;
            foreach (var kv in expandoDict1)
            {
                hashCode *= -1521134295 + kv.Key.GetHashCode();
                hashCode *= -1521134295 + EqualityComparer<object>.Default.GetHashCode(kv.Value);
            }

            return hashCode;
        }

        public static int GetHashCode(object item) =>
            item is ExpandoObject expandoItem ?
            GetHashCode(expandoItem) :
            item?.GetHashCode() ?? 0;
    }
}
