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
        public async Task<List<PlaceholderMetadata>> GetPlaceholdersAsync(string? id = null, string? group = null, string? name = null, string? type = null, string? description = null)
        {
            var queryParams = new List<string>();
            if (!string.IsNullOrWhiteSpace(id))
                queryParams.Add($"id={Uri.EscapeDataString(id)}");
            if (!string.IsNullOrWhiteSpace(group))
                queryParams.Add($"group={Uri.EscapeDataString(group)}");
            if (!string.IsNullOrWhiteSpace(name))
                queryParams.Add($"name={Uri.EscapeDataString(name)}");
            if (!string.IsNullOrWhiteSpace(type))
                queryParams.Add($"type={Uri.EscapeDataString(type)}");
            if (!string.IsNullOrWhiteSpace(description))
                queryParams.Add($"description={Uri.EscapeDataString(description)}");

            var url = "/api/contract-generation/placeholders/filter";
            if (queryParams.Count > 0)
                url += "?" + string.Join("&", queryParams);

            var response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<PlaceholderMetadata>>() ?? new();
            }
            return new();
        }
    }
}
