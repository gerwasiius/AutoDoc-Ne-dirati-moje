using System.Net.Http.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoDoc.Shared.Model.DTO.SectionGroupDTO;
using AutoDoc.Shared.Model.Placeholders;

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
        /// Gets all placeholders grouped by group (identical to backend response).
        /// </summary>
        public async Task<List<PlaceholderGroup>> GetAllPlaceholderGroupsAsync()
        {
            var url = "/api/contract-generation/placeholders";
            var response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<PlaceholderGroup>>() ?? new();
            }
            return new();
        }

        /// <summary>
        /// Gets a single placeholder by its ID.
        /// </summary>
        public async Task<PlaceholderMetadata?> GetPlaceholderByIdAsync(string id)
        {
            var url = $"/api/contract-generation/placeholders/{Uri.EscapeDataString(id)}";
            var response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PlaceholderMetadata>();
            }
            return null;
        }
    }
}
