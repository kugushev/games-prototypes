using System.Collections.Generic;

namespace Kugushev.Scripts.Common.Utils
{
    public static class StringBag
    {
        private static readonly Dictionary<int, string> IntToStringBag = new Dictionary<int, string>(128);

        public static string FromInt(int value)
        {
            if (!IntToStringBag.TryGetValue(value, out var text))
                text = IntToStringBag[value] = value.ToString();
            return text;
        }
    }
}