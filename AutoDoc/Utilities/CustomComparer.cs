namespace AutoDocFront.Utilities
{
    public class CustomComparer : IComparer<object?>
    {
        private readonly bool _ascending;

        public CustomComparer(bool ascending)
        {
            _ascending = ascending;
        }

        public int Compare(object? x, object? y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x is null) return _ascending ? -1 : 1;
            if (y is null) return _ascending ? 1 : -1;

            if (x is IComparable xComparable && y is IComparable yComparable)
            {
                return _ascending ? xComparable.CompareTo(yComparable) : yComparable.CompareTo(xComparable);
            }

            throw new ArgumentException("At least one object must implement IComparable.");
        }
    }
}
