using AutoDoc.Shared.Model.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoDoc.Shared.Model.Placeholders
{
    /// <summary>
    /// Predstavlja meta podatke jednog placeholdera koji se koristi u šablonima dokumenata.
    /// </summary>
    [Serializable]
    public class PlaceholderMetadata
    {
        /// <summary>
        /// Jedinstveni identifikator placeholdera (npr. "Group1.Placeholder1").
        /// </summary>
        [Required]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Naziv grupe kojoj placeholder pripada.
        /// </summary>
        [Required]
        public string Group { get; set; } = string.Empty;

        /// <summary>
        /// Prikazni naziv placeholdera.
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Tip placeholdera (npr. "string", "int", "enum").
        /// </summary>
        [Required]
        public PlaceholderType Type { get; set; }

        /// <summary>
        /// Opis placeholdera.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Označava da li je vrijednost placeholdera opcionalna (nullable).
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Lista mogućih vrijednosti (ako je tip "enum"), u suprotnom null.
        /// </summary>
        public IReadOnlyList<string>? EnumValues { get; set; }

        /// <summary>
        /// Označava da li je placeholder tipa enum.
        /// </summary>
        public bool IsEnum => EnumValues != null && EnumValues.Count > 0;

        /// <summary>
        /// Prazan konstruktor za serializaciju.
        /// </summary>
        public PlaceholderMetadata() { }

        /// <summary>
        /// Konstruktor za inicijalizaciju svih svojstava.
        /// </summary>
        public PlaceholderMetadata(
            string id,
            string group,
            string name,
            PlaceholderType type,
            string? description,
            bool isNullable,
            IReadOnlyList<string>? enumValues = null)
        {
            Id = id;
            Group = group;
            Name = name;
            Type = type;
            Description = description;
            IsNullable = isNullable;
            EnumValues = enumValues;
        }

        /// <summary>
        /// Vraća string reprezentaciju placeholder meta podataka.
        /// </summary>
        public override string ToString()
            => $"{Id} ({Type}){(IsNullable ? "?" : "")}";
    }
}
