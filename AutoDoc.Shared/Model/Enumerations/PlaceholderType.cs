using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AutoDoc.Shared.Model.Enumerations
{
    /// <summary>
    /// Tip placeholdera za logiku uslova i prikaz korisniku.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PlaceholderType
    {
        /// <summary>Tekstualni podatak.</summary>
        STRING,
        /// <summary>Cijeli broj.</summary>
        INT,
        /// <summary>Decimalni broj.</summary>
        DECIMAL,
        /// <summary>Logička vrijednost.</summary>
        BOOL,
        /// <summary>Enumeracija.</summary>
        ENUM,
        /// <summary>Datum i vrijeme.</summary>
        DATETIME,
        /// <summary>CHAR.</summary>
        CHAR,
        /// <summary>Neodređeno/ostalo.</summary>
        UNKNOWN
    }
}
