using AutoDoc.Shared.Model.DTO.Enumerations;
using System.Text.Json.Serialization;

namespace AutoDoc.Shared.Model.DTO.DocumentTemplateDTO
{
    //// <summary>
    /// DTO za prikaz predloška dokumenta.
    /// </summary>
    public class DocumentTemplateGetDTO
    {
        /// <summary>
        /// Jedinstveni identifikator predloška.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Logički identifikator predloška.
        /// </summary>
        public int IdTemplate { get; set; }

        /// <summary>
        /// Broj verzije predloška.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Naziv predloška.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Opis predloška.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Status predloška.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DocumentTemplateStatusType? Status { get; set; }

        /// <summary>
        /// Datum od kada je predložak važeći.
        /// </summary>
        public DateTime? ValidFrom { get; set; }

        /// <summary>
        /// Datum do kada je predložak važeći.
        /// </summary>
        public DateTime? ValidTo { get; set; }

        /// <summary>
        /// Datum unosa predloška.
        /// </summary>
        public DateTime? DateInserted { get; set; }

        /// <summary>
        /// Korisnik koji je unio predložak.
        /// </summary>
        public string? UserInserted { get; set; }

        /// <summary>
        /// Datum posljednje izmjene predloška.
        /// </summary>
        public DateTime? DateUpdated { get; set; }

        /// <summary>
        /// Korisnik koji je posljednji put ažurirao predložak.
        /// </summary>
        public string? UserUpdated { get; set; }

        /// <summary>
        /// Datum verifikacije predloška.
        /// </summary>
        public DateTime? DateVerified { get; set; }

        /// <summary>
        /// Korisnik koji je verifikovao predložak.
        /// </summary>
        public string? UserVerified { get; set; }
    }
}
