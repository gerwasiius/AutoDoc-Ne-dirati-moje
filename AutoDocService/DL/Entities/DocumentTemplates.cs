namespace AutoDocService.DL.Entities
{
    /// <summary>
    /// Tablea koja sluzi za spremanje Document Template-a
    /// </summary>
    public partial class DocumentTemplates
    {
        /// <summary>
        /// Jedinstveni identifikator za predložak dokumenta
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Logički identifikator za predložak
        /// </summary>
        public int IdTemplate { get; set; }

        /// <summary>
        /// Ime predloška
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Opis predloška
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Broj verzije predloška
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Trenutni status predloška
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Datum od kada je predložak važeći
        /// </summary>
        public DateTime? ValidFrom { get; set; }

        /// <summary>
        /// Datum do kada je predložak važeći
        /// </summary>
        public DateTime? ValidTo { get; set; }

        /// <summary>
        /// Datum kada je predložak unesen
        /// </summary>
        public DateTime DateInserted { get; set; }

        /// <summary>
        /// Korisnik koji je unio predložak
        /// </summary>
        public string UserInserted { get; set; }

        /// <summary>
        /// Datum kada je predložak posljednji put ažuriran
        /// </summary>
        public DateTime? DateUpdated { get; set; }

        /// <summary>
        /// Korisnik koji je posljednji put ažurirao predložak
        /// </summary>
        public string UserUpdated { get; set; }

        /// <summary>
        /// Datum kada je predložak verificiran
        /// </summary>
        public DateTime? DateVerified { get; set; }

        /// <summary>
        /// Korisnik koji je verificirao predložak
        /// </summary>
        public string UserVerified { get; set; }

        /// <summary>
        /// Lista TemplatesectionsRelacije
        /// </summary>
        public virtual ICollection<TemplateSectionsRelation> TemplateSectionsRelations { get; set; } = new List<TemplateSectionsRelation>();

    }
}