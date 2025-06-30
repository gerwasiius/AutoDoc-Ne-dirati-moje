namespace AutoDoc.Shared.Model.DTO.Common
{
    /// <summary>
    /// Class created to make pagination of list of objects
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T>
    {
        /// <summary>
        /// List of objects returned from response
        /// </summary>
        public List<T> Items { get; set; } = new();
        /// <summary>
        /// Amount of objects that will show up
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// How many objects should be skipped
        /// </summary>
        public int Offset { get; set; }
        /// <summary>
        /// Number of objects that should be skipped
        /// </summary>
        public int? NextPageOffset { get; set; }
        /// <summary>
        /// Previous number of objects that are skipped
        /// </summary>
        public int PreviousPageOffset { get; set; }
        /// <summary>
        /// Total number of items
        /// </summary>
        public int TotalItems { get; set; }


        // Prazan konstruktor za deserializaciju
        public PagedList() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="items"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        public PagedList(List<T> items, int limit, int offset, int totalItems)
        {
            PageSize = limit;
            Offset = offset;
            TotalItems = totalItems;
            Items = items;

            CalculateNextPageOffset();
            CalculatePreviousPageOffset();
        }

        /// <summary>
        /// Calculation of  NextPageOffseta
        /// </summary>
        private void CalculateNextPageOffset()
        {
            NextPageOffset = TotalItems > (Offset + PageSize) ? Offset + PageSize : null;
        }

        /// <summary>
        /// Calculation of PreviousPageOffset
        /// </summary>
        private void CalculatePreviousPageOffset()
        {
            PreviousPageOffset = Offset - PageSize >= 0 ? Offset - PageSize : 0;
        }
    }
}
