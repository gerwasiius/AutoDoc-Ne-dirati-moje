using AutoDocFront.Models.DTO;
using AutoDocFront.Models.DTO.DocumentTemplateDTO;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace AutoDocFront.Services;

/// <summary>
/// Service that centralizes API calls for document templates.
/// </summary>
public class DocumentTemplateApiService
{
    private readonly HttpClient _client;
    private readonly ILogger<DocumentTemplateApiService> _logger;

    public DocumentTemplateApiService(IHttpClientFactory httpClientFactory, ILogger<DocumentTemplateApiService> logger)
    {
        _client = httpClientFactory.CreateClient("AutoDocService");
        _logger = logger;
    }

    /// <summary>
    /// Retrieves document templates.
    /// </summary>
    public async Task<PagedList<DocumentTemplateGetDTO>> GetTemplatesAsync(int offset = 0, int pageSize = 0)
    {
        var url = $"/api/contract-generation/document-templates?offset={offset}&pageSize={pageSize}";
        var response = await _client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<PagedList<DocumentTemplateGetDTO>>()
                   ?? new PagedList<DocumentTemplateGetDTO>();
        }

        _logger.LogWarning("Failed to load templates. Status code: {Status}", response.StatusCode);
        return new PagedList<DocumentTemplateGetDTO> { Items = new List<DocumentTemplateGetDTO>(), TotalItems = 0 };
    }

    /// <summary>
    /// Creates a new document template.
    /// </summary>
    public async Task<bool> CreateTemplateAsync(DocumentTemplateCreateDTO dto)
    {
        var response = await _client.PostAsJsonAsync("/api/contract-generation/document-templates", dto);
        return response.IsSuccessStatusCode;
    }

    /// <summary>
    /// Updates an existing template.
    /// </summary>
    public async Task<bool> UpdateTemplateAsync(int id, DocumentTemplateUpdateDTO dto)
    {
        var response = await _client.PutAsJsonAsync($"/api/contract-generation/document-templates/{id}", dto);
        return response.IsSuccessStatusCode;
    }
}
