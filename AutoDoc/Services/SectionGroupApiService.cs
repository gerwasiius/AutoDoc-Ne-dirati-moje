using AutoDoc.Shared.Model.DTO.Common;
using AutoDoc.Shared.Model.DTO.SectionGroupDTO;
using AutoDoc.Shared.Model.DTO.SectionsDTO;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace AutoDocFront.Services
{
    /// <summary>
    /// Service that centralizes API calls for section group management.
    /// </summary>
    public class SectionGroupApiService
    {
        private readonly HttpClient _client;
        private readonly ILogger<SectionGroupApiService> _logger;

        public SectionGroupApiService(IHttpClientFactory httpClientFactory, ILogger<SectionGroupApiService> logger)
        {
            _client = httpClientFactory.CreateClient("AutoDocService");
            _logger = logger;
        }

        /// <summary>
        /// Retrieves section groups with optional search and status filters.
        /// </summary>
        public async Task<PagedList<SectionGroupGetDTO>> GetGroupsAsync(string? name, string status, int offset, int pageSize)
        {
            var query = new List<string>
            {
                $"offset={offset}",
                $"pageSize={pageSize}"
            };

            if (!string.IsNullOrWhiteSpace(name))
                query.Add($"name={Uri.EscapeDataString(name)}");
            if (!string.IsNullOrWhiteSpace(status) && status != "all")
                query.Add($"status={status}");

            var url = "/api/contract-generation/section-groups";
            if (query.Count > 0)
                url += "?" + string.Join("&", query);

            var response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PagedList<SectionGroupGetDTO>>()
                       ?? new PagedList<SectionGroupGetDTO>();
            }

            _logger.LogWarning("Failed to load section groups. Status code: {Status}", response.StatusCode);
            return new PagedList<SectionGroupGetDTO> { Items = new List<SectionGroupGetDTO>(), TotalItems = 0 };
        }

        /// <summary>
        /// Dohvata jednu grupu po ID-u.
        /// </summary>
        public async Task<SectionGroupGetDTO?> GetGroupByIdAsync(int id)
        {
            var url = $"/api/contract-generation/section-groups?id={id}&status=ACTIVE";
            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;

            var paged = await response.Content.ReadFromJsonAsync<PagedList<SectionGroupGetDTO>>();
            return paged?.Items?.FirstOrDefault();
        }

        /// <summary>
        /// Creates a new section group.
        /// </summary>
        public async Task<bool> CreateGroupAsync(SectionGroupCreateDTO dto)
        {
            var response = await _client.PostAsJsonAsync("/api/contract-generation/section-groups", dto);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Updates an existing section group.
        /// </summary>
        public async Task<bool> UpdateGroupAsync(SectionGroupUpdateDTO dto)
        {
            var response = await _client.PutAsJsonAsync("/api/contract-generation/section-groups", dto);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Checks if a group has any active sections.
        /// </summary>
        public async Task<bool> HasActiveSectionsAsync(int groupId)
        {
            var response = await _client.GetAsync($"/api/contract-generation/sections?groupId={groupId}&isActive=true&pageSize=1");
            if (!response.IsSuccessStatusCode)
                return false;

            var result = await response.Content.ReadFromJsonAsync<PagedList<SectionsGetDTO>>();
            return result != null && result.Items.Any();
        }
    }
}