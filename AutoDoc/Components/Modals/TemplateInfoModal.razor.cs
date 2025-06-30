using AutoDoc.Shared.Model.DTO.DocumentTemplateDTO;
using AutoDoc.Shared.Model.DTO.Enumerations;
using AutoDocFront.Models.Enumerations;
using AutoDocFront.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;

namespace AutoDocFront.Components.Modals
{
    /// <summary>
    /// Modal za unos, izmjenu i pregled dokument template-a.
    /// </summary>
    public partial class TemplateInfoModal
    {
        // --- PARAMETRI ---

        /// <summary>
        /// Da li je modal otvoren.
        /// </summary>
        [Parameter] public bool IsOpen { get; set; }

        /// <summary>
        /// Event za promjenu stanja otvaranja modala.
        /// </summary>
        [Parameter] public EventCallback<bool> IsOpenChanged { get; set; }

        /// <summary>
        /// Template za prikaz ili izmjenu.
        /// </summary>
        [Parameter] public DocumentTemplateGetDTO Template { get; set; }

        /// <summary>
        /// Režim rada modala (unos, izmjena, pregled).
        /// </summary>
        [Parameter] public ModalMode ModalMode { get; set; }

        /// <summary>
        /// Event koji se poziva nakon uspješnog snimanja.
        /// </summary>
        [Parameter] public EventCallback OnSave { get; set; }

        /// <summary>
        /// Event koji se poziva za prebacivanje u edit režim.
        /// </summary>
        [Parameter] public EventCallback OnEdit { get; set; }

        // --- INJECTION ---

        /// <summary>
        /// Servis za rad sa dokument template-ima.
        /// </summary>
        [Inject] private DocumentTemplateApiService TemplateService { get; set; } = default!;

        /// <summary>
        /// Servis za prikaz notifikacija.
        /// </summary>
        [Inject] private IToastService ToastService { get; set; } = default!;

        // --- POLJA ---

        private EditContext _editContext;
        private ValidationMessageStore _validationMessageStore;
        private DocumentTemplateGetDTO _model;
        private bool _loading = false;
        private bool _isUnlimitedValidTo = false;

        private string _modalStyle => IsOpen ? "display: block;" : "display: none;";

        /// <summary>
        /// Naslov modala na osnovu režima.
        /// </summary>
        private string ModalTitle => ModalMode switch
        {
            ModalMode.EDIT => "Izmjena predloška",
            ModalMode.VIEW => "Pregled predloška",
            _ => "Unos novog predloška"
        };

        /// <summary>
        /// Inicijalizuje model i EditContext na osnovu parametara i režima modala.
        /// </summary>
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

            _isUnlimitedValidTo = !_model.ValidTo.HasValue;
            _editContext = new EditContext(_model);
            _validationMessageStore = new ValidationMessageStore(_editContext);
        }

        /// <summary>
        /// Zatvara modal i emituje promjenu stanja.
        /// </summary>
        private async Task CloseModalAsync()
        {
            IsOpen = false;
            await IsOpenChanged.InvokeAsync(false);
        }

        /// <summary>
        /// Validira formu i izvršava submit (insert ili update template-a).
        /// </summary>
        private async Task HandleValidSubmitAsync()
        {
            _validationMessageStore.Clear();

            if (_editContext.Validate())
            {
                _loading = true;
                if (ModalMode == ModalMode.EDIT)
                {
                    await UpdateTemplateAsync();
                }
                else
                {
                    await InsertTemplateAsync();
                }
                _loading = false;
            }
            else
            {
                _editContext.NotifyValidationStateChanged();
                ToastService.ShowError("Provjerite da li su sva polja ispravno unesena!");
            }
        }

        /// <summary>
        /// Kreira novi template.
        /// </summary>
        private async Task InsertTemplateAsync()
        {
            try
            {
                var createDTO = new DocumentTemplateCreateDTO
                {
                    Name = _model.Name,
                    Description = _model.Description,
                    Status = DocumentTemplateStatusType.IN_PROGRESS,
                    ValidFrom = _model.ValidFrom,
                    ValidTo = _isUnlimitedValidTo ? null : _model.ValidTo,
                    UserInserted = "zlatan.kahriman" // TODO: Zamijeniti sa stvarnim korisnikom
                };

                var (isSuccess, _, errorMessage) = await TemplateService.CreateTemplateAsync(createDTO);

                if (isSuccess)
                {
                    ToastService.ShowSuccess("Predložak uspješno kreiran!");
                    await CloseModalAsync();
                    await OnSave.InvokeAsync();
                }
                else
                {
                    ToastService.ShowError(errorMessage ?? "Greška prilikom kreiranja predloška!");
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Neočekivana greška: {ex.Message}");
            }
        }

        /// <summary>
        /// Ažurira postojeći template.
        /// </summary>
        private async Task UpdateTemplateAsync()
        {
            try
            {
                var updateDTO = new DocumentTemplateUpdateDTO
                {
                    Name = _model.Name,
                    Description = _model.Description,
                    Status = _model.Status,
                    ValidFrom = _model.ValidFrom,
                    ValidTo = _isUnlimitedValidTo ? null : _model.ValidTo,
                    UserUpdated = "zlatan.kahriman" // TODO: Zamijeniti sa stvarnim korisnikom
                };

                var (isSuccess, _, errorMessage) = await TemplateService.UpdateTemplateAsync(_model.Id, updateDTO);

                if (isSuccess)
                {
                    ToastService.ShowSuccess("Predložak uspješno ažuriran!");
                    await CloseModalAsync();
                    await OnSave.InvokeAsync();
                }
                else
                {
                    ToastService.ShowError(errorMessage ?? "Greška prilikom ažuriranja predloška!");
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Neočekivana greška: {ex.Message}");
            }
        }

        /// <summary>
        /// Postavlja ili uklanja neograničen datum ValidTo.
        /// </summary>
        /// <param name="e">Event sa novom vrijednošću.</param>
        private void ToggleUnlimitedValidTo(ChangeEventArgs e)
        {
            _isUnlimitedValidTo = (bool)e.Value;
            if (_isUnlimitedValidTo)
            {
                _model.ValidTo = null;
            }
            StateHasChanged();
        }
    }
}
