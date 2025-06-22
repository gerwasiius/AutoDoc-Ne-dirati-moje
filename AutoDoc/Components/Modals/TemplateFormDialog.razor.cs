using AutoDocFront.Components.Pages;
using AutoDocFront.Models.DTO;
using AutoDocFront.Models.DTO.DocumentTemplate;
using AutoDocFront.Models.DTO.DocumentTemplateDTO;
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
        //[Parameter] public EventCallback<Template> FormDataChanged { get; set; }
        //[Parameter] public Template? EditingTemplate { get; set; }
        [Parameter] public EventCallback OnSubmit { get; set; }
        [Parameter] public EventCallback OpenSectionPicker { get; set; }
        [Parameter] public EventCallback<(int, int)> MoveSection { get; set; }
        [Parameter] public EventCallback<int> RemoveSection { get; set; }
        [Parameter] public EventCallback<int> OpenSectionConditions { get; set; }
        [Parameter] public EventCallback<int> PreviewSection { get; set; }
        [Parameter] public DocumentTemplateGetDTO Template { get; set; }
        [Parameter] public EventCallback OnClose { get; set; }

        // --- INJECTION ---

        /// <summary>
        /// Fabrika za kreiranje HttpClient instanci.
        /// </summary>
        [Inject] private IHttpClientFactory HttpClientFactory { get; set; }

        /// <summary>
        /// Servis za prikaz notifikacija (toast poruka).
        /// </summary>
        [Inject] private IToastService ToastService { get; set; }

        /// <summary>
        /// Servis za prikaz dijaloga.
        /// </summary>
        [Inject] private IDialogService DialogService { get; set; }
        private DocumentTemplateAndRelatedItemsDTO templateWithSections;
        private bool loading = false;

        private async Task OpenSectionPickerClicked() => await OpenSectionPicker.InvokeAsync();
        private async Task MoveSectionClicked(int idx, int direction) => await MoveSection.InvokeAsync((idx, direction));
        private async Task RemoveSectionClicked(int idx) => await RemoveSection.InvokeAsync(idx);
        private async Task OpenSectionConditionsClicked(int idx) => await OpenSectionConditions.InvokeAsync(idx);
        private async Task PreviewSectionClicked(int idx) => await PreviewSection.InvokeAsync(idx);
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
