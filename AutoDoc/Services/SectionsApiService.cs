using AutoDoc.Shared.Model.DTO.Common;
using AutoDoc.Shared.Model.DTO.SectionsDTO;
using AutoDocFront.Models.Enumerations;
using System.Net;

public class SectionsApiService
{
    private readonly HttpClient _client;
    public SectionsApiService(IHttpClientFactory factory) => _client = factory.CreateClient("AutoDocService");

    /// <summary>
    /// Kreira novu sekciju (prva verzija).
    /// </summary>
    public async Task<(bool IsSuccess, HttpStatusCode StatusCode, string? ErrorMessage)> InsertSectionAsync(SectionsCreateDTO dto)
    {
        var response = await _client.PostAsJsonAsync("/api/contract-generation/sections", dto);
        if (response.IsSuccessStatusCode)
            return (true, response.StatusCode, null);

        var errorMsg = await response.Content.ReadAsStringAsync();
        return (false, response.StatusCode, errorMsg);
    }

    /// <summary>
    /// Ažurira postojeću sekciju (kreira novu verziju).
    /// </summary>
    public async Task<(bool IsSuccess, HttpStatusCode StatusCode, string? ErrorMessage)> UpdateSectionAsync(int id, SectionsUpdateDTO dto)
    {
        var response = await _client.PutAsJsonAsync($"/api/contract-generation/sections/{id}/manage-section", dto);
        if (response.IsSuccessStatusCode)
            return (true, response.StatusCode, null);

        var errorMsg = await response.Content.ReadAsStringAsync();
        return (false, response.StatusCode, errorMsg);
    }

    /// <summary>
    /// Vraća sve verzije sekcije po IdSection.
    /// </summary>
    public async Task<List<SectionsGetDTO>?> GetAllVersionsForSectionAsync(int idSection)
    {
        var url = $"/api/contract-generation/sections?idSection={idSection}&offset=0&pageSize=0";
        var response = await _client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return null;

        var paged = await response.Content.ReadFromJsonAsync<PagedList<SectionsGetDTO>>();
        return paged?.Items ?? new List<SectionsGetDTO>();
    }

    /// <summary>
    /// Ažurira status sekcije (aktivacija/deaktivacija).
    /// </summary>
    public async Task<bool> UpdateSectionStatusAsync(int? id, int? sectionId, bool isActiveStatus)
    {
        var url = $"/api/contract-generation/sections/update-status?sectionId={sectionId}&isActiveStatus={isActiveStatus}";
        var response = await _client.PatchAsync(url, null);
        return response.IsSuccessStatusCode;
    }

    /// <summary>
    /// Dohvata sekcije sa paginacijom i filtrima.
    /// </summary>
    public async Task<PagedList<SectionsGetDTO>> GetSectionsAsync(int groupId, string? name, SectionStatusType? status, int offset, int pageSize)
    {
        var query = new List<string>
        {
            $"groupId={groupId}",
            "isLatestOnly=true",
            $"offset={offset}",
            $"pageSize={pageSize}"
        };
        if (!string.IsNullOrWhiteSpace(name))
            query.Add($"name={Uri.EscapeDataString(name)}");
        if (status == SectionStatusType.ACTIVE)
            query.Add("isActive=true");
        else if (status == SectionStatusType.DEACTIVATED)
            query.Add("isActive=false");

        var url = "/api/contract-generation/sections?" + string.Join("&", query);
        var response = await _client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            throw new Exception("Problem prilikom učitavanja sekcija");

        return await response.Content.ReadFromJsonAsync<PagedList<SectionsGetDTO>>() ?? new PagedList<SectionsGetDTO>();
    }

    /// <summary>
    /// Dohvata sve posljednje verzije sekcija za datu grupu i opcione statuse.
    /// </summary>
    public async Task<List<SectionsGetDTO>> GetAllLatestSectionsAsync(int groupId, HashSet<SectionStatusType> statuses, int pageSize = 50)
    {
        var result = new List<SectionsGetDTO>();
        int offset = 0;
        PagedList<SectionsGetDTO>? paged;
        do
        {
            SectionStatusType? status = null;
            if (statuses != null && statuses.Count == 1)
                status = statuses.First();

            var page = await GetSectionsAsync(groupId, null, status, offset, pageSize);
            if (page.Items != null)
                result.AddRange(page.Items);
            offset = page.NextPageOffset ?? 0;
            paged = page;
        } while (paged?.NextPageOffset != null);

        return result;
    }
}
