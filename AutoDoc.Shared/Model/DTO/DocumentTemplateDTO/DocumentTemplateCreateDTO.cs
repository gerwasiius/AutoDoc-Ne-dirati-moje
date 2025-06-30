using AutoDoc.Shared.Model.DTO.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AutoDoc.Shared.Model.DTO.DocumentTemplateDTO
{
    /// <summary>
    /// DTO za kreiranje predloška dokumenta.
    /// </summary>
    public class DocumentTemplateCreateDTO
    {
        /// <summary>
        /// Logički identifikator predloška (opciono).
        /// </summary>
        public int? IdTemplate { get; set; }

        /// <summary>
        /// Broj verzije predloška.
        /// </summary>
        [Required(ErrorMessage = "Verzija je obavezna.")]
        public int Version { get; set; }

        /// <summary>
        /// Naziv predloška.
        /// </summary>
        [Required(ErrorMessage = "Naziv predloška je obavezan.")]
        [StringLength(100, ErrorMessage = "Naziv može imati najviše 100 karaktera.")]
        public string Name { get; set; }

        /// <summary>
        /// Opis predloška.
        /// </summary>
        [Required(ErrorMessage = "Opis predloška je obavezan.")]
        [StringLength(250, ErrorMessage = "Opis može imati najviše 250 karaktera.")]
        public string Description { get; set; }

        /// <summary>
        /// Status predloška.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required(ErrorMessage = "Status predloška je obavezan.")]
        public DocumentTemplateStatusType? Status { get; set; }

        /// <summary>
        /// Datum od kada je predložak važeći.
        /// </summary>
        [Required(ErrorMessage = "Datum od kada je predložak važeći je obavezan.")]
        public DateTime? ValidFrom { get; set; }

        /// <summary>
        /// Datum do kada je predložak važeći (opciono).
        /// </summary>
        public DateTime? ValidTo { get; set; }

        /// <summary>
        /// Korisnik koji je unio predložak.
        /// </summary>
        [Required(ErrorMessage = "Korisnik koji unosi predložak je obavezan.")]
        public string UserInserted { get; set; }
    }
}
