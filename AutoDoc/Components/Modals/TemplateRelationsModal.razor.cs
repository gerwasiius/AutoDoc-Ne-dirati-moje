using AutoDoc.Shared.Model.DTO.Common;
using AutoDoc.Shared.Model.DTO.DocumentTemplateDTO;
using AutoDoc.Shared.Model.DTO.SectionsDTO;
using AutoDoc.Shared.Model.DTO.TemplateSectionsRelationDTO;
using AutoDocFront.Components.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace AutoDocFront.Components.Modals
{
    public partial class TemplateRelationsModal
    {
        [Parameter] public bool IsOpen { get; set; }
        [Parameter] public EventCallback<bool> IsOpenChanged { get; set; }
        [Parameter] public DocumentTemplateAndRelatedItemsDTO FormData { get; set; } = new();
        [Parameter] public DocumentTemplateGetDTO Template { get; set; }
        [Parameter] public EventCallback OnSubmit { get; set; }
        [Parameter] public EventCallback OnClose { get; set; }

        [Inject] private IHttpClientFactory HttpClientFactory { get; set; }
        [Inject] private IToastService ToastService { get; set; }
        //[Inject] private IDialogService DialogService { get; set; }

        private DocumentTemplateAndRelatedItemsDTO templateWithSections;
        private bool _isLoading = false;
        private bool isSectionPickerOpen = false;


        private bool isPreviewModalOpen = false;
        private string previewHtmlContent;
        private bool previewLoading = false;
        private string previewError;
        private string previewTemplateName;
        private bool isConditionModalOpen = false;
        private int? editingRelationIdx = null;

        




        // --- Sekcije logika unutar modala ---

        private async Task OpenSectionPicker()
        {
            // Implementacija otvaranja pickera sekcija
            // npr. prikazivanje internog dijaloga ili logike
            isSectionPickerOpen = true;
        }

        private async Task OnSectionsPicked(List<SectionsGetDTO> pickedSections)
        {
            // Dodaj odabrane sekcije u FormData.Relations
            if (FormData?.Relations == null)
                FormData.Relations = new List<TemplateSectionRelationWithSectionDTO>();

            // Pronađi trenutni maksimalni Order (ili 0 ako nema sekcija)
            int currentMaxOrder = FormData.Relations.Any()
                ? FormData.Relations.Max(r => r.Order)
                : 0;

            foreach (var section in pickedSections)
            {
                if (!FormData.Relations.Any(r => r.SectionUniqueId == section.ID))
                {
                    currentMaxOrder++; // Dodajemo na kraj

                    FormData.Relations.Add(new TemplateSectionRelationWithSectionDTO
                    {
                        SectionUniqueId = section.ID,
                        SectionId = section.IdSection ?? 0,
                        SectionVersion = section.Version,
                        SectionName = section.Name,
                        SectionDescription = section.Description,
                        Order = currentMaxOrder
                    });
                }
            }
            isSectionPickerOpen = false;
            StateHasChanged();
        }

        private void CloseSectionPicker()
        {
            isSectionPickerOpen = false;
        }

        private async Task MoveSection(int idx, int direction)
        {
            if (FormData?.Relations == null) return;
            int newIndex = idx + direction;
            if (newIndex < 0 || newIndex >= FormData.Relations.Count) return;

            // Swap the items
            var item = FormData.Relations[idx];
            FormData.Relations.RemoveAt(idx);
            FormData.Relations.Insert(newIndex, item);

            // Update Order for all items to match their new position
            for (int i = 0; i < FormData.Relations.Count; i++)
            {
                FormData.Relations[i].Order = i + 1; // Order is usually 1-based
            }

            StateHasChanged();
        }

        private void RemoveSection(int idx)
        {
            if (FormData?.Relations == null || idx < 0 || idx >= FormData.Relations.Count)
                return;

            FormData.Relations.RemoveAt(idx);
            StateHasChanged();
        }

        private async Task PreviewSection(int idx)
        {
            if (FormData?.Relations == null || idx < 0 || idx >= FormData.Relations.Count)
                return;

            var section = FormData.Relations[idx];
            previewTemplateName = section.SectionName;
            previewLoading = true;
            previewError = null;
            previewHtmlContent = null;
            isPreviewModalOpen = true;

            try
            {
                var client = HttpClientFactory.CreateClient("AutoDocService");
                // Pripremi payload kao lista sa jednom sekcijom
                var singleSectionList = new List<TemplateSectionRelationWithSectionDTO> { section };
                var response = await client.PostAsJsonAsync(
                    "/api/document-render/preview",
                    singleSectionList
                );

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<PreviewResponse>();
                    previewHtmlContent = result?.htmlContent;
                }
                else
                {
                    previewError = "Greška prilikom dohvata pregleda sekcije.";
                }
            }
            catch (Exception ex)
            {
                previewError = $"Greška: {ex.Message}";
            }
            finally
            {
                previewLoading = false;
                StateHasChanged();
            }
        }

        private async Task Submit()
        {
            try
            {
                _isLoading = true;
                var client = HttpClientFactory.CreateClient("AutoDocService");
                var response = await client.PostAsJsonAsync(
                    "/api/contract-generation/template-sections-relations/manage-relations",
                    FormData);

                if (response.IsSuccessStatusCode)
                {
                    ToastService.ShowSuccess("Template sekcije su uspješno sačuvane!");
                    await OnSubmit.InvokeAsync();
                    await IsOpenChanged.InvokeAsync(false); // zatvori modal
                }
                else
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    ToastService.ShowError($"Greška prilikom čuvanja: {errorMsg}");
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Neočekivana greška: {ex.Message}");
            }
            finally
            {
                _isLoading = false;
                await OnSubmit.InvokeAsync();
            }
        }

        private async Task Close() => await IsOpenChanged.InvokeAsync(false);

        protected override async Task OnParametersSetAsync()
        {
            _isLoading = true;
            try
            {
                var client = HttpClientFactory.CreateClient("AutoDocService");
                var response = await client.GetAsync($"/api/contract-generation/document-templates/template-items?id={Template.Id}");
                if (response.IsSuccessStatusCode)
                {
                    var paged = await response.Content.ReadFromJsonAsync<PagedList<DocumentTemplateAndRelatedItemsDTO>>();
                    FormData = paged?.Items?.FirstOrDefault();
                }
            }
            finally
            {
                _isLoading = false;
            }
        }


        private async Task ShowPreview()
        {
            previewTemplateName = FormData.Name;
            previewLoading = true;
            previewError = null;
            previewHtmlContent = null;
            isPreviewModalOpen = true;

            try
            {
                var client = HttpClientFactory.CreateClient("AutoDocService");
                var response = await client.PostAsJsonAsync(
                    "/api/document-render/preview",
                    FormData.Relations // List<TemplateSectionRelationWithSectionDTO>
                );

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<PreviewResponse>();
                    previewHtmlContent = result?.htmlContent;
                }
                else
                {
                    previewError = "Greška prilikom dohvata pregleda.";
                }
            }
            catch (Exception ex)
            {
                previewError = $"Greška: {ex.Message}";
            }
            finally
            {
                previewLoading = false;
                StateHasChanged();
            }
        }

        private void OpenConditionModal(int idx)
        {
            editingRelationIdx = idx;
            isConditionModalOpen = true;
        }

        private void CloseConditionModal()
        {
            isConditionModalOpen = false;
            editingRelationIdx = null;
        }

        private class PreviewResponse
        {
            public string htmlContent { get; set; }
        }
    }
}