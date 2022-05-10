using System;

namespace WatchableData.Utils
{
    public static class ComparableExtensions
    {
        public static bool IsGreaterThan<T>(this T value, T other)
            where T : IComparable
        {
            return value.CompareTo(other) > 0;
        }

        public static bool IsLessThan<T>(this T value, T other)
            where T : IComparable
        {
            return value.CompareTo(other) < 0;
        }
    }
}
