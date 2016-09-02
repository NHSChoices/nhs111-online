using System.Collections.Generic;

namespace NHS111.Utils.Extensions
{
    public static class ListExtension
    {
        public static List<T> InList<T>(this T item)
        {
            return new List<T> { item };
        }
    }
}