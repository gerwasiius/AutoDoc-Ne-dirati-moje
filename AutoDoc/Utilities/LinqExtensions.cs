namespace AutoDocFront.Utilities
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> ThenBy<T>(this IOrderedEnumerable<T> source, Func<T, IComparable?> keySelector, bool ascending)
        {
            return ascending ? source.ThenBy(keySelector) : source.ThenByDescending(keySelector);
        }
    }
}
