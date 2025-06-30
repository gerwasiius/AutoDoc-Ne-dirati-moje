namespace AutoDoc.Shared.Model.DTO.SectionsDTO
{
    /// <summary>
    /// DTO za prikaz sekcije (čitanje/pregled).
    /// </summary>
    public class SectionsGetDTO
    {
        /// <summary>
        /// Unificirani identifikator sekcije (verzija).
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Logički identifikator sekcije (sve verzije iste sekcije imaju isti IdSection).
        /// </summary>
        public int? IdSection { get; set; }

        /// <summary>
        /// Identifikator grupe kojoj sekcija pripada.
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Naziv sekcije.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Opis sekcije.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Sadržaj sekcije.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Označava da li je sekcija aktivna.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Broj verzije sekcije.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Datum kada je sekcija unesena.
        /// </summary>
        public DateTime? DateInserted { get; set; }

        /// <summary>
        /// Korisnik koji je unio sekciju.
        /// </summary>
        public string UserInserted { get; set; }

        /// <summary>
        /// Datum kada je sekcija posljednji put ažurirana.
        /// </summary>
        public DateTime? DateUpdated { get; set; }

        /// <summary>
        /// Korisnik koji je posljednji put ažurirao sekciju.
        /// </summary>
        public string UserUpdated { get; set; }
    }
}
