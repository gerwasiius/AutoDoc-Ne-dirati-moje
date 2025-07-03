using AutoDoc.Shared.Model.DTO.Common;
using AutoDoc.Shared.Model.DTO.DocumentTemplateDTO;
using AutoDoc.Shared.Model.DTO.SectionsDTO;
using AutoDoc.Shared.Model.DTO.TemplateSectionsRelationDTO;
using AutoDocFront.Components.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using AutoDocFront.Services;

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

        [Inject] private DocumentTemplateApiService TemplateService { get; set; } = default!;
        [Inject] private IToastService ToastService { get; set; } = default!;
        //[Inject] private IDialogService DialogService { get; set; }

        private bool _isLoading = false;
        private bool _isSectionPickerOpen = false;


        private bool _isPreviewModalOpen = false;
        private string _previewHtmlContent;
        private bool _previewLoading = false;
        private string _previewError;
        private string _previewTemplateName;
        private bool _isConditionModalOpen = false;
        private int? _editingRelationIdx = null;

        




        // --- Sekcije logika unutar modala ---

        private async Task OpenSectionPicker()
        {
            // Implementacija otvaranja pickera sekcija
            // npr. prikazivanje internog dijaloga ili logike
            _isSectionPickerOpen = true;
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
            _isSectionPickerOpen = false;
            StateHasChanged();
        }

        private void CloseSectionPicker()
        {
            _isSectionPickerOpen = false;
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
            _previewTemplateName = section.SectionName;
            _previewLoading = true;
            _previewError = null;
            _previewHtmlContent = null;
            _isPreviewModalOpen = true;

            try
            {
                var html = await TemplateService.GetSectionsPreviewAsync(new List<TemplateSectionRelationWithSectionDTO> { section });
                if (html != null)
                {
                    _previewHtmlContent = html;
                }
                else
                {
                    _previewError = "Greška prilikom dohvata pregleda sekcije.";
                }
            }
            catch (Exception ex)
            {
                _previewError = $"Greška: {ex.Message}";
            }
            finally
            {
                _previewLoading = false;
                StateHasChanged();
            }
        }

        private async Task Submit()
        {
            try
            {
                _isLoading = true;
                var (isSuccess, _, error) = await TemplateService.SaveTemplateSectionsAsync(FormData);

                if (isSuccess)
                {
                    ToastService.ShowSuccess("Template sekcije su uspješno sačuvane!");
                    await OnSubmit.InvokeAsync();
                    await IsOpenChanged.InvokeAsync(false);
                }
                else
                {
                    ToastService.ShowError(error ?? "Greška prilikom čuvanja");
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
                var result = await TemplateService.GetTemplateWithSectionsAsync(Template.Id);
                if (result != null)
                {
                    FormData = result;
                }
            }
            finally
            {
                _isLoading = false;
            }
        }


        private async Task ShowPreview()
        {
            _previewTemplateName = FormData.Name;
            _previewLoading = true;
            _previewError = null;
            _previewHtmlContent = null;
            _isPreviewModalOpen = true;

            try
            {
                var html = await TemplateService.GetSectionsPreviewAsync(FormData.Relations);
                if (html != null)
                {
                    _previewHtmlContent = html;
                }
                else
                {
                    _previewError = "Greška prilikom dohvata pregleda.";
                }
            }
            catch (Exception ex)
            {
                _previewError = $"Greška: {ex.Message}";
            }
            finally
            {
                _previewLoading = false;
                StateHasChanged();
            }
        }

        private void OpenConditionModal(int idx)
        {
            _editingRelationIdx = idx;
            _isConditionModalOpen = true;
        }

        private void CloseConditionModal()
        {
            _isConditionModalOpen = false;
            _editingRelationIdx = null;
        }

        private class PreviewResponse
        {
            public string htmlContent { get; set; }
        }
    }
}