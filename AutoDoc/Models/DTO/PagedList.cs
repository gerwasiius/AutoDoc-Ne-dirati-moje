namespace AutoDocFront.Models.DTO
{

    /// <summary>
    /// Klasa napravljena kako bi se izvrsavala paginacija za liste
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T>
    {
        /// <summary>
        /// Lista koja se vraca iz response-a.
        /// </summary>
        public List<T>? Items { get; set; }
        /// <summary>
        /// Iznos koliko ce se objekata prikazati
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Koliko objekata da preskoci
        /// </summary>
        public int Offset { get; set; }
        /// <summary>
        /// Koji je sljedeci broj objekata koji bi trebalo preskociti.
        /// </summary>
        public int? NextPageOffset { get; set; }
        /// <summary>
        /// Prethodni broj objekata koji su preskoceni.
        /// </summary>
        public int PreviousPageOffset { get; set; }
        /// <summary>
        /// Ukupan iznos objekata
        /// </summary>
        public int TotalItems { get; set; }

    }
}
