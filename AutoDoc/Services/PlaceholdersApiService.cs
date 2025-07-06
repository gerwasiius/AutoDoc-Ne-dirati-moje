using System.Net.Http.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using static AutoDocFront.Components.Pages.Placeholders;
using AutoDoc.Shared.Model.Placeholders.PlaceholderMetadata;
using AutoDoc.Shared.Model.DTO.SectionGroupDTO;

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
        public async Task<List<PlaceholderMeta>> GetPlaceholdersAsync(string searchTerm)
        {
            var url = "/api/contract-generation/placeholders";
            var response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<PlaceholderMeta>>()
                       ?? new List<PlaceholderMeta>();
            }
            else
            {
                return null;
            }
        }
    }
}
