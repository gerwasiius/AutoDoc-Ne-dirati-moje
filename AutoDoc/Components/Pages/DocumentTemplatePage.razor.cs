using AutoDocFront.Models.DTO;
using AutoDocFront.Models.DTO.DocumentTemplateDTO;
using AutoDocFront.Models.Enumerations;
using AutoDocFront.Services;
using Microsoft.AspNetCore.Components;

namespace AutoDocFront.Components.Pages
{
    public partial class DocumentTemplatePage
    {
        private List<DocumentTemplateGetDTO> templates = new();
        private DocumentTemplateGetDTO selectedTemplate;
        private bool _loading = false;
        private bool isModalOpen = false;
        private ModalMode modalMode;

        protected override async Task OnInitializedAsync()
        {
            await LoadTemplatesAsync();
        }

        private async Task LoadTemplatesAsync()
        {
            try
            {
                _loading = true;
                var pagedList = await TemplateService.GetTemplatesAsync(0, 0);
                templates = pagedList.Items?.OrderByDescending(t => t.Version).ToList() ?? new();
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
