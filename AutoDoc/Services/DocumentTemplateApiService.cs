using AutoDocFront.Models.Enumerations;
using System.Net.Http.Json;
using System.Net;
using AutoDoc.Shared.Model.DTO.Enumerations;
using AutoDoc.Shared.Model.DTO.DocumentTemplateDTO;
using AutoDoc.Shared.Model.DTO.Common;
using AutoDoc.Shared.Model.DTO.TemplateSectionsRelationDTO;

namespace AutoDocFront.Services
{
    /// <summary>
    /// Servis za rad sa dokument template-ima i njihovim sekcijama.
    /// </summary>
    public class DocumentTemplateApiService
    {
        private readonly HttpClient _client;

        public DocumentTemplateApiService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("AutoDocService");
        }

        /// <summary>
        /// Dohvata template-e sa paginacijom i filtrima.
        /// </summary>
        public async Task<PagedList<DocumentTemplateGetDTO>> GetTemplatesAsync(
            string? name = null,
            DocumentTemplateStatusType? status = null,
            int offset = 0,
            int pageSize = 30)
        {
            var query = new List<string>
            {
                $"offset={offset}",
                $"pageSize={pageSize}"
            };
            if (!string.IsNullOrWhiteSpace(name))
                query.Add($"name={Uri.EscapeDataString(name)}");
            if (status.HasValue)
                query.Add($"status={status.Value}");

            var url = "/api/contract-generation/document-templates";
            if (query.Count > 0)
                url += "?" + string.Join("&", query);

            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Problem prilikom učitavanja template-a");

            return await response.Content.ReadFromJsonAsync<PagedList<DocumentTemplateGetDTO>>() ?? new PagedList<DocumentTemplateGetDTO>();
        }

        /// <summary>
        /// Kreira novi template.
        /// </summary>
        public async Task<(bool IsSuccess, HttpStatusCode StatusCode, string? ErrorMessage)> CreateTemplateAsync(DocumentTemplateCreateDTO dto)
        {
            var response = await _client.PostAsJsonAsync("/api/contract-generation/document-templates", dto);
            if (response.IsSuccessStatusCode)
                return (true, response.StatusCode, null);

            var errorMsg = await response.Content.ReadAsStringAsync();
            return (false, response.StatusCode, errorMsg);
        }

        /// <summary>
        /// Ažurira postojeći template.
        /// </summary>
        public async Task<(bool IsSuccess, HttpStatusCode StatusCode, string? ErrorMessage)> UpdateTemplateAsync(int id, DocumentTemplateUpdateDTO dto)
        {
            var response = await _client.PutAsJsonAsync($"/api/contract-generation/document-templates/{id}", dto);
            if (response.IsSuccessStatusCode)
                return (true, response.StatusCode, null);

            var errorMsg = await response.Content.ReadAsStringAsync();
            return (false, response.StatusCode, errorMsg);
        }

        /// <summary>
        /// Dohvata template sa svim povezanim sekcijama (za prikaz u formi).
        /// </summary>
        public async Task<DocumentTemplateAndRelatedItemsDTO?> GetTemplateWithSectionsAsync(int templateId)
        {
            var url = $"/api/contract-generation/document-templates/template-items?id={templateId}";
            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;

            var paged = await response.Content.ReadFromJsonAsync<PagedList<DocumentTemplateAndRelatedItemsDTO>>();
            return paged?.Items?.FirstOrDefault();
        }

        /// <summary>
        /// Snima relacije između template-a i sekcija.
        /// </summary>
        public async Task<(bool IsSuccess, HttpStatusCode StatusCode, string? ErrorMessage)> SaveTemplateSectionsAsync(DocumentTemplateAndRelatedItemsDTO dto)
        {
            var response = await _client.PostAsJsonAsync("/api/contract-generation/template-sections-relations/manage-relations", dto);
            if (response.IsSuccessStatusCode)
                return (true, response.StatusCode, null);

            var errorMsg = await response.Content.ReadAsStringAsync();
            return (false, response.StatusCode, errorMsg);
        }

        /// <summary>
        /// Prikazuje preview template-a (HTML render).
        /// </summary>
        public async Task<string?> GetTemplatePreviewAsync(int idTemplate, int version)
        {
            var url = $"/api/document-render/{idTemplate}/render?version={version}";
            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<TemplatePreviewResult>();
            return result?.HtmlContent;
        }

        /// <summary>
        /// Prikazuje preview za listu sekcija (HTML render).
        /// </summary>
        public async Task<string?> GetSectionsPreviewAsync(List<TemplateSectionRelationWithSectionDTO> relations)
        {
            var response = await _client.PostAsJsonAsync("/api/document-render/preview", relations);
            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<TemplatePreviewResult>();
            return result?.HtmlContent;
        }

        private class TemplatePreviewResult
        {
            public string HtmlContent { get; set; }
        }
    }
}
