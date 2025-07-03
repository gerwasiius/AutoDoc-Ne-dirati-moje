using Newtonsoft.Json;

namespace AutoDocFront.Services
{
    public class DocumentRenderApiService
    {
        private readonly HttpClient _client;

        public DocumentRenderApiService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("PdfService");
        }


        public class PdfGenHtmlTextRequest : PdfGenPartial
        {
            public string Html { get; set; }
        }

        public partial class PdfGenPartial
        {
            public List<ImageByte> ImageByte { get; set; } = null;
            public AdditionalPDFSettings AdditionalPdfSettings { get; set; } = null;
        }

        public class ImageByte
        {
            public byte[] ImagesByteArray { set; get; }
            public string ImageExtensions { set; get; }
        }

        /// <summary>
        /// Dodatne klase za stiliziranje PDF-a
        /// </summary>
        public class AdditionalPDFSettings
        {
            public Dictionary<string, object> GlobalSettings { get; set; } = null;
            public Dictionary<string, object> ObjectSettings { get; set; } = null;
            public Dictionary<string, object> WebSettings { get; set; } = null;
            public Dictionary<string, object> HeaderSettings { get; set; } = null;
            public Dictionary<string, object> FooterSettings { get; set; } = null;
            public Margins Margins { get; set; } = null;
        }

        /// <summary>
        /// Dodavanje margina na PDF-u, defaultno je null.
        /// </summary>
        public class Margins
        {
            public double? Top { get; set; } = null;
            public double? Right { get; set; } = null;
            public double? Bottom { get; set; } = null;
            public double? Left { get; set; } = null;
        }
    }
}
