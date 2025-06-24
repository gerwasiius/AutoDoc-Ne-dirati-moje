using AutoDocFront.Components.Pages;
using AutoDocFront.Models.DTO;
using AutoDocFront.Models.DTO.DocumentTemplate;
using AutoDocFront.Models.DTO.DocumentTemplateDTO;
using AutoDocFront.Models.DTO.Relations;
using AutoDocFront.Models.DTO.Sections;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace AutoDocFront.Components.Modals
{
    public partial class TemplateFormDialog
    {
        [Parameter] public bool IsOpen { get; set; }
        [Parameter] public EventCallback<bool> IsOpenChanged { get; set; }
        [Parameter] public DocumentTemplateAndRelatedItemsDTO FormData { get; set; } = new();
        [Parameter] public DocumentTemplateGetDTO Template { get; set; }
        [Parameter] public EventCallback OnSubmit { get; set; }
        [Parameter] public EventCallback OnClose { get; set; }

        [Inject] private IHttpClientFactory HttpClientFactory { get; set; }
        [Inject] private IToastService ToastService { get; set; }
        [Inject] private IDialogService DialogService { get; set; }

        private DocumentTemplateAndRelatedItemsDTO templateWithSections;
        private bool loading = false;
        private bool isSectionPickerOpen = false;

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

            foreach (var section in pickedSections)
            {
                if (!FormData.Relations.Any(r => r.Section.ID == section.ID))
                {
                    FormData.Relations.Add(new TemplateSectionRelationWithSectionDTO
                    {
                        Section = section
                        // Popuni ostale potrebne podatke ako treba
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
            // Implementacija pomjeranja sekcije u listi
            if (FormData?.Relations == null) return;
            int newIndex = idx + direction;
            if (newIndex < 0 || newIndex >= FormData.Relations.Count) return;

            var item = FormData.Relations[idx];
            FormData.Relations.RemoveAt(idx);
            FormData.Relations.Insert(newIndex, item);
            StateHasChanged();
        }

        private void RemoveSection(int idx)
        {
            if (FormData?.Relations == null || idx < 0 || idx >= FormData.Relations.Count)
                return;

            FormData.Relations.RemoveAt(idx);
            StateHasChanged();
        }

        private async Task OpenSectionConditions(int idx)
        {
            // Implementacija otvaranja uslova za sekciju
            // npr. prikazivanje internog dijaloga
        }

        private async Task PreviewSection(int idx)
        {
            // Implementacija pregleda sekcije
            // npr. prikazivanje preview dijaloga
        }

        private async Task Submit() => await OnSubmit.InvokeAsync();
        private async Task Close() => await IsOpenChanged.InvokeAsync(false);

        protected override async Task OnParametersSetAsync()
        {
            loading = true;
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
                loading = false;
            }
        }
    }
}
