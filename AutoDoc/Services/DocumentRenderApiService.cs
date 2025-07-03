using Newtonsoft.Json;

namespace AutoDocFront.Services
{
    /// <summary>
    /// Servis koji komunicira sa PDF servisom za generisanje dokumenata.
    /// </summary>
    public class DocumentRenderApiService
    {
        private readonly HttpClient _client;

        /// <summary>
        /// Inicijalizuje servis i kreira HttpClient za PDF servis.
        /// </summary>
        public DocumentRenderApiService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("PdfService");
        }
    }
}
