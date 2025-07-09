using AutoDoc.Shared.Model.DTO.Common;
using AutoDoc.Shared.Model.DTO.DocumentTemplateDTO;
using AutoDoc.Shared.Model.DTO.SectionsDTO;
using AutoDoc.Shared.Model.DTO.TemplateSectionsRelationDTO;
using AutoDocFront.Components.Shared;
using AutoDocFront.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace AutoDocFront.Components.Modals
{
    public partial class TemplateRelationsModal : ModalBase
    {
        [Parameter] public DocumentTemplateAndRelatedItemsDTO FormData { get; set; } = new();
        [Parameter] public DocumentTemplateGetDTO Template { get; set; }
        [Parameter] public EventCallback OnSubmit { get; set; }
        [Parameter] public EventCallback OnClose { get; set; }

        [Inject] private DocumentTemplateApiService TemplateService { get; set; } = default!;
        [Inject] private IToastService ToastService { get; set; } = default!;
        //[Inject] private IDialogService DialogService { get; set; }

        private DocumentTemplateAndRelatedItemsDTO templateWithSections;
        private bool _isLoading = false;
        private bool _isSectionPickerOpen = false;


        private bool _isPreviewModalOpen = false;
        private string _previewHtmlContent = string.Empty;
        private bool _previewLoading = false;
        private string _previewError = string.Empty;
        private string _previewTemplateName = string.Empty;
        private bool _isConditionModalOpen = false;
        private int? _editingRelationIdx = null;






        // --- Sekcije logika unutar modala ---

        private async Task OpenSectionPicker()
        {
            try
            {
                // Implementacija otvaranja pickera sekcija
                // npr. prikazivanje internog dijaloga ili logike
                _isSectionPickerOpen = true;
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom otvaranja pickera sekcija: {ex.Message}");
            }
        }

        private async Task OnSectionsPicked(List<SectionsGetDTO> pickedSections)
        {
            try
            {
                if (FormData?.Relations == null)
                    FormData.Relations = new List<TemplateSectionRelationWithSectionDTO>();

                int currentMaxOrder = FormData.Relations.Any()
                    ? FormData.Relations.Max(r => r.Order)
                    : 0;

                foreach (var section in pickedSections)
                {
                    if (!FormData.Relations.Any(r => r.SectionUniqueId == section.ID))
                    {
                        currentMaxOrder++;
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
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom dodavanja sekcija: {ex.Message}");
            }
        }

        private void CloseSectionPicker()
        {
            try
            {
                _isSectionPickerOpen = false;
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom zatvaranja pickera sekcija: {ex.Message}");
            }
        }


        private async Task MoveSection(int idx, int direction)
        {
            try
            {
                if (FormData?.Relations == null) return;
                int newIndex = idx + direction;
                if (newIndex < 0 || newIndex >= FormData.Relations.Count) return;

                var item = FormData.Relations[idx];
                FormData.Relations.RemoveAt(idx);
                FormData.Relations.Insert(newIndex, item);

                for (int i = 0; i < FormData.Relations.Count; i++)
                {
                    FormData.Relations[i].Order = i + 1;
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom promjene redosleda sekcija: {ex.Message}");
            }
        }

        private void RemoveSection(int idx)
        {
            try
            {
                if (FormData?.Relations == null || idx < 0 || idx >= FormData.Relations.Count)
                    return;

                FormData.Relations.RemoveAt(idx);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom uklanjanja sekcije: {ex.Message}");
            }
        }

        private async Task PreviewSection(int idx)
        {
            if (FormData?.Relations == null || idx < 0 || idx >= FormData.Relations.Count)
                return;

            _previewTemplateName = FormData.Relations[idx].SectionName;
            _previewLoading = true;
            _previewError = null;
            _previewHtmlContent = null;
            _isPreviewModalOpen = true;

            try
            {
                var singleSectionList = new List<TemplateSectionRelationWithSectionDTO> { FormData.Relations[idx] };
                var html = await TemplateService.GetSectionsPreviewAsync(singleSectionList);
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
                    await CloseAsync();
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
                // OnSubmit se ne bi trebao pozivati u finally, samo na uspeh!
            }
        }

        private async Task Close()
        {
            try
            {
                await CloseAsync();
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom zatvaranja modala: {ex.Message}");
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            _isLoading = true;
            try
            {
                if (Template == null)
                {
                    ToastService.ShowError("Nije moguće učitati sekcije: Template nije definisan.");
                    return;
                }
                var result = await TemplateService.GetTemplateWithSectionsAsync(Template.Id);
                if (result != null)
                {
                    FormData = result;
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom učitavanja template-a i sekcija: {ex.Message}");
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
            try
            {
                _editingRelationIdx = idx;
                _isConditionModalOpen = true;
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom otvaranja modala za uslove: {ex.Message}");
            }
        }

        private void CloseConditionModal()
        {
            try
            {
                _isConditionModalOpen = false;
                _editingRelationIdx = null;
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom zatvaranja modala za uslove: {ex.Message}");
            }
        }

        private class PreviewResponse
        {
            public string htmlContent { get; set; }
        }
    }
}