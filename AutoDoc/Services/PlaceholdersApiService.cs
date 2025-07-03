using System.Net.Http.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using static AutoDocFront.Components.Pages.Placeholders;

namespace AutoDocFront.Services
{
    public class PlaceholdersApiService
    {
        private readonly HttpClient _client;

        public PlaceholdersApiService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("AutoDocService");
        }

        /// <summary>
        /// Gets all placeholders, optionally filtered by search term.
        /// </summary>
        public async Task<List<PlaceholderDTO>> GetPlaceholdersAsync(string searchTerm)
        {
            var url = "/api/placeholders";
            var result = await _client.GetFromJsonAsync<List<PlaceholderDTO>>(url) ?? new List<PlaceholderDTO>();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var lower = searchTerm.ToLowerInvariant();
                result = result
                    .Where(p => (!string.IsNullOrEmpty(p.Name) && p.Name.ToLowerInvariant().Contains(lower))
                             || (!string.IsNullOrEmpty(p.Description) && p.Description.ToLowerInvariant().Contains(lower)))
                    .ToList();
            }

            return result;
        }
    }
}
