namespace AutoDocFront.Utilities
{
    /// <summary>
    /// Utility methods for common pagination calculations.
    /// </summary>
    public class PaginationHelper
    {
        /// <summary>
        /// Calculates total pages for the given item count and page size.
        /// </summary>
        public static int CalculateTotalPages(int totalItems, int pageSize)
        {
            return (int)Math.Ceiling((double)totalItems / pageSize);
        }

        /// <summary>
        /// Calculates the start index for the given page.
        /// </summary>
        public static int CalculateStartIndex(int currentPage, int pageSize, int totalItems)
        {
            return totalItems == 0 ? 0 : (currentPage - 1) * pageSize;
        }

        /// <summary>
        /// Calculates the end index for the given start index and count.
        /// </summary>
        public static int CalculateEndIndex(int startIndex, int count, int totalItems)
        {
            return Math.Min(startIndex + count, totalItems);
        }
    }
}