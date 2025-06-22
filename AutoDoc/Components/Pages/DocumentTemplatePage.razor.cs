using AutoDocFront.Models.DTO;
using AutoDocFront.Models.DTO.DocumentTemplateDTO;
using AutoDocFront.Models.Enumerations;
using Microsoft.AspNetCore.Components;

namespace AutoDocFront.Components.Pages
{
    public partial class DocumentTemplatePage
    {
        private HttpClient _autoDocServiceClient;
        private List<DocumentTemplateGetDTO> templates = new();
        private DocumentTemplateGetDTO selectedTemplate;
        private bool _loading = false;
        private bool isModalOpen = false;
        private ModalMode modalMode;

        protected override async Task OnInitializedAsync()
        {
            _autoDocServiceClient = httpClientFactory.CreateClient("AutoDocService");
            await LoadTemplatesAsync();
        }

        private async Task LoadTemplatesAsync()
        {
            try
            {
                _loading = true;
                var response = await _autoDocServiceClient.GetAsync("/api/contract-generation/document-templates?offset=0&pageSize=0");
                if (response.IsSuccessStatusCode)
                {
                    var pagedList = await response.Content.ReadFromJsonAsync<PagedList<DocumentTemplateGetDTO>>();
                    templates = pagedList?.Items?.OrderByDescending(t => t.Version).ToList() ?? new();
                }
                else
                {
                    toastService.ShowError("Failed to load document templates!");
                }
            }
            catch(Exception ex)
            {
                throw new Exception("problem");
            }
            finally
            {
                _loading = false;
            }
        }

        private void SelectTemplate(DocumentTemplateGetDTO template)
        {
            selectedTemplate = template;
        }

        private void ShowCreateModal()
        {
            selectedTemplate = null;
            modalMode = ModalMode.INSERT;
            isModalOpen = true;
        }

        private void ShowEditModal(DocumentTemplateGetDTO template)
        {
            selectedTemplate = template;
            modalMode = ModalMode.EDIT;
            isModalOpen = true;
        }

        private async Task OnModalSave()
        {
            await LoadTemplatesAsync();
        }

        private void ClosePrompt()
        {
            NavigationManager.NavigateTo("/");
        }
    }
}
