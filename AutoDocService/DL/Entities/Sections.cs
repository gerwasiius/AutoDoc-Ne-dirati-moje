namespace AutoDocService.DL.Entities
{
    /// <summary>
    /// Tabela za Clanove/Sekcije
    /// </summary>
    public partial class Sections
    {
        /// <summary>
        /// Jedinstveni identifikator za tabelu sekcija
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Logički identifikator za sekciju
        /// </summary>
        public int IdSection { get; set; }

        /// <summary>
        /// Oznaka kojoj grupi ovaj clan pripada
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Ime sekcije
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Opis sekcije
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Sadržaj sekcije
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Broj verzije sekcije
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Označava da li je sekcija aktivna
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Datum kada je sekcija unesena
        /// </summary>
        public DateTime? DateInserted { get; set; }

        /// <summary>
        /// Korisnik koji je unio sekciju
        /// </summary>
        public string UserInserted { get; set; }

        /// <summary>
        /// Datum kada je sekcija posljednji put ažurirana
        /// </summary>
        public DateTime? DateUpdated { get; set; }

        /// <summary>
        /// Korisnik koji je posljednji put ažurirao sekciju
        /// </summary>
        public string UserUpdated { get; set; }
        /// <summary>
        /// Lista TemplateSection relacija
        /// </summary>
        public virtual ICollection<TemplateSectionsRelation> TemplateSectionsRelations { get; set; } = new List<TemplateSectionsRelation>();

    }
}