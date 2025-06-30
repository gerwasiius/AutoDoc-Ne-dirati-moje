using AutoDoc.Shared.Model.DTO.Enumerations;

namespace AutoDoc.Shared.Model.DTO.SectionGroupDTO
{
    /// <summary>
    /// DTO za prikaz grupe sekcija.
    /// </summary>
    public class SectionGroupGetDTO
    {
        /// <summary>
        /// Jedinstveni identifikator grupe.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Naziv grupe sekcija.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Opis grupe sekcija.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Status of section's group
        /// </summary>
        public GroupStatusType Status { get; set; }

        /// <summary>
        /// Datum kreiranja grupe.
        /// </summary>
        public DateTime? DateInserted { get; set; }

        /// <summary>
        /// Korisnik koji je kreirao grupu.
        /// </summary>
        public string UserInserted { get; set; }

        /// <summary>
        /// Datum posljednje izmjene grupe.
        /// </summary>
        public DateTime? DateUpdated { get; set; }

        /// <summary>
        /// Korisnik koji je posljednji put ažurirao grupu.
        /// </summary>
        public string UserUpdated { get; set; }
    }
}
