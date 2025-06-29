using AutoDocFront.Models.DTO.DocumentTemplateDTO;
using AutoDocFront.Models.Enumerations;
using AutoDocFront.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AutoDocFront.Components.Modals
{
    public partial class DocumentTemplateModal
    {
        [Parameter] public bool IsOpen { get; set; }
        [Parameter] public EventCallback<bool> IsOpenChanged { get; set; }
        [Parameter] public DocumentTemplateGetDTO Template { get; set; }
        [Parameter] public ModalMode ModalMode { get; set; }
        [Parameter] public EventCallback OnSave { get; set; }
        [Parameter] public EventCallback OnEdit { get; set; }

        [Inject] private DocumentTemplateApiService TemplateService { get; set; }
        private EditContext _editContext;
        private ValidationMessageStore _validationMessageStore;
        private DocumentTemplateGetDTO _model;
        private DocumentTemplateGetDTO selectedTemplate;
        private bool _loading = false;
        public bool isModalOpen = false;
        public ModalMode modalMode;
        private string _modalStyle => IsOpen ? "display: block;" : "display: none;";

        private string ModalTitle => ModalMode switch
        {
            ModalMode.EDIT => "Izmjena predloška",
            ModalMode.VIEW => "Pregled predloška",
            _ => "Unos novog predloška"
        };

        protected override void OnParametersSet()
        {

            if ((ModalMode == ModalMode.EDIT || ModalMode == ModalMode.VIEW) && Template != null)
            {
                _model = new DocumentTemplateGetDTO
                {
                    Id = Template.Id,
                    IdTemplate = Template.IdTemplate,
                    Name = Template.Name,
                    Description = Template.Description,
                    Status = Template.Status,
                    ValidFrom = Template.ValidFrom,
                    ValidTo = Template.ValidTo
                };
            }
            else
            {
                _model = new DocumentTemplateGetDTO
                {
                    Status = DocumentTemplateStatusType.IN_PROGRESS
                };
            }

            _editContext = new EditContext(_model);
            _validationMessageStore = new ValidationMessageStore(_editContext);
        }

        private void CloseModal()
        {
            IsOpen = false;
            IsOpenChanged.InvokeAsync(false);
        }

        private async Task HandleValidSubmit()
        {
            _validationMessageStore.Clear();

            if (_editContext.Validate())
            {
                _loading = true;
                if (ModalMode == ModalMode.EDIT)
                {
                    await UpdateTemplate();
                }
                else
                {
                    await InsertTemplate();
                }
                _loading = false;
            }
            else
            {
                _editContext.NotifyValidationStateChanged();
                toastService.ShowError("Provjerite da li su sva polja ispravno unesena!");
            }
        }

        private async Task InsertTemplate()
        {
            try
            {
                var createDTO = new DocumentTemplateCreateDTO
                {
                    Name = _model.Name,
                    Description = _model.Description,
                    Status = DocumentTemplateStatusType.IN_PROGRESS,
                    ValidFrom = _model.ValidFrom,
                    ValidTo = _model.ValidTo,
                    UserInsert = "zlatan.kahriman"
                };

                var success = await TemplateService.CreateTemplateAsync(createDTO);

                if (success)
                {
                    toastService.ShowSuccess("Predložak uspješno kreiran!");
                    CloseModal();
                    await OnSave.InvokeAsync();
                }
                else
                {
                    toastService.ShowError("Greška prilikom kreiranja predloška!");
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Neočekivana greška: {ex.Message}");
            }
        }

        private async Task UpdateTemplate()
        {
            try
            {
                var updateDTO = new DocumentTemplateUpdateDTO
                {
                    Name = _model.Name,
                    Description = _model.Description,
                    Status = _model.Status,
                    ValidFrom = _model.ValidFrom,
                    ValidTo = _model.ValidTo
                };

                var success = await TemplateService.UpdateTemplateAsync(_model.Id, updateDTO);

                if (success)
                {
                    toastService.ShowSuccess("Predložak uspješno ažuriran!");
                    CloseModal();
                    await OnSave.InvokeAsync();
                }
                else
                {
                    toastService.ShowError("Greška prilikom ažuriranja predloška!");
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Neočekivana greška: {ex.Message}");
            }
        }

        private void ShowCreateModal()
        {
            selectedTemplate = null;
            modalMode = ModalMode.INSERT;
            isModalOpen = true;
        }

        private void ShowEditModal()
        {
            modalMode = ModalMode.EDIT;
            isModalOpen = true;
        }

        private async Task OnModalSave()
        {
            //await LoadTemplatesAsync();
        }

        private bool isUnlimitedValidTo = false;

        private void ToggleUnlimitedValidTo(ChangeEventArgs e)
        {
            isUnlimitedValidTo = (bool)e.Value;
            if (isUnlimitedValidTo)
            {
                _model.ValidTo = null;
            }
            StateHasChanged();
        }
    }
}