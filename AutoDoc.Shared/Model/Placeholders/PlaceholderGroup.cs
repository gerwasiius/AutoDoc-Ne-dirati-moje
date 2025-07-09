using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoDoc.Shared.Model.Placeholders
{
    public class PlaceholderGroup
    {
        /// <summary>
        /// Grupa placeholdera
        /// </summary>
        public string Group { get; set; }
        /// <summary>
        /// Detalji o placeholderima
        /// </summary>
        public List<PlaceholderMetadata> Placeholders { get; set; }
    }
}
