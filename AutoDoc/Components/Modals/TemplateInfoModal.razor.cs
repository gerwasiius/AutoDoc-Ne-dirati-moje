using AutoDoc.Shared.Model.DTO.DocumentTemplateDTO;
using AutoDoc.Shared.Model.DTO.Enumerations;
using AutoDocFront.Components.Shared;
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
    public partial class TemplateInfoModal : ModalBase
    {
        // --- PARAMETRI ---

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
        private DocumentTemplateCreateDTO _createModel = new();
        private DocumentTemplateUpdateDTO _updateModel = new();
        private DocumentTemplateGetDTO _viewModel = new();
        private bool _isLoading = false;

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
            try
            {
                if (ModalMode == ModalMode.INSERT)
                {
                    _createModel = new DocumentTemplateCreateDTO
                    {
                        Status = DocumentTemplateStatusType.IN_PROGRESS,
                        ValidFrom = DateTime.Today
                    };
                    _isUnlimitedValidTo = true;
                    _editContext = new EditContext(_createModel);
                }
                else if (ModalMode == ModalMode.EDIT && Template != null)
                {
                    _updateModel = new DocumentTemplateUpdateDTO
                    {
                        Name = Template.Name,
                        Description = Template.Description,
                        Status = Template.Status,
                        ValidFrom = Template.ValidFrom,
                        ValidTo = Template.ValidTo
                    };
                    _isUnlimitedValidTo = !_updateModel.ValidTo.HasValue;
                    _editContext = new EditContext(_updateModel);
                }
                else if (ModalMode == ModalMode.VIEW && Template != null)
                {
                    _viewModel = Template;
                    _isUnlimitedValidTo = !_viewModel.ValidTo.HasValue;
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom inicijalizacije modala: {ex.Message}");
            }
        }

        // Zatvaranje modala implementirano je u baznoj klasi.

        /// <summary>
        /// Validira formu i izvršava submit (insert ili update template-a).
        /// </summary>
        private async Task HandleValidSubmitAsync()
        {
            if (_editContext is null) return;

            _createModel.UserInserted = "zlatan.kahriman"; // TODO: zamijeniti stvarnim korisnikom
            _updateModel.UserUpdated = "zlatan.kahriman"; // TODO: zamijeniti stvarnim korisnikom

            if (_editContext.Validate())
            {
                _isLoading = true;
                try
                {
                    if (ModalMode == ModalMode.INSERT)
                    {
                        _createModel.ValidTo = _isUnlimitedValidTo ? null : _createModel.ValidTo;
                        _createModel.UserInserted = "zlatan.kahriman"; // TODO: zamijeniti stvarnim korisnikom
                        var (isSuccess, _, errorMessage) = await TemplateService.CreateTemplateAsync(_createModel);

                        if (isSuccess)
                        {
                            ToastService.ShowSuccess("Predložak uspješno kreiran!");
                            await CloseAsync();
                            await OnSave.InvokeAsync();
                        }
                        else
                        {
                            ToastService.ShowError(errorMessage ?? "Greška prilikom kreiranja predloška!");
                        }
                    }
                    else if (ModalMode == ModalMode.EDIT && Template != null)
                    {
                        _updateModel.ValidTo = _isUnlimitedValidTo ? null : _updateModel.ValidTo;
                        _updateModel.UserUpdated = "zlatan.kahriman"; // TODO: zamijeniti stvarnim korisnikom
                        var (isSuccess, _, errorMessage) = await TemplateService.UpdateTemplateAsync(Template.Id, _updateModel);

                        if (isSuccess)
                        {
                            ToastService.ShowSuccess("Predložak uspješno ažuriran!");
                            await CloseAsync();
                            await OnSave.InvokeAsync();
                        }
                        else
                        {
                            ToastService.ShowError(errorMessage ?? "Greška prilikom ažuriranja predloška!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ToastService.ShowError($"Neočekivana greška: {ex.Message}");
                }
                finally
                {
                    _isLoading = false;
                }
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
        //private async Task InsertTemplateAsync()
        //{
        //    try
        //    {
        //        var createDTO = new DocumentTemplateCreateDTO
        //        {
        //            Name = .Name,
        //            Description = _model.Description,
        //            Status = DocumentTemplateStatusType.IN_PROGRESS,
        //            ValidFrom = _model.ValidFrom,
        //            ValidTo = _isUnlimitedValidTo ? null : _model.ValidTo,
        //            UserInserted = "zlatan.kahriman" // TODO: Zamijeniti sa stvarnim korisnikom
        //        };

        //        var (isSuccess, _, errorMessage) = await TemplateService.CreateTemplateAsync(createDTO);

        //        if (isSuccess)
        //        {
        //            ToastService.ShowSuccess("Predložak uspješno kreiran!");
        //            await CloseAsync();
        //            await OnSave.InvokeAsync();
        //        }
        //        else
        //        {
        //            ToastService.ShowError(errorMessage ?? "Greška prilikom kreiranja predloška!");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ToastService.ShowError($"Neočekivana greška: {ex.Message}");
        //    }
        //}

        /// <summary>
        /// Ažurira postojeći template.
        /// </summary>
        //private async Task UpdateTemplateAsync()
        //{
        //    try
        //    {
        //        var updateDTO = new DocumentTemplateUpdateDTO
        //        {
        //            Name = _model.Name,
        //            Description = _model.Description,
        //            Status = _model.Status,
        //            ValidFrom = _model.ValidFrom,
        //            ValidTo = _isUnlimitedValidTo ? null : _model.ValidTo,
        //            UserUpdated = "zlatan.kahriman" // TODO: Zamijeniti sa stvarnim korisnikom
        //        };

        //        var (isSuccess, _, errorMessage) = await TemplateService.UpdateTemplateAsync(_model.Id, updateDTO);

        //        if (isSuccess)
        //        {
        //            ToastService.ShowSuccess("Predložak uspješno ažuriran!");
        //            await CloseAsync();
        //            await OnSave.InvokeAsync();
        //        }
        //        else
        //        {
        //            ToastService.ShowError(errorMessage ?? "Greška prilikom ažuriranja predloška!");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ToastService.ShowError($"Neočekivana greška: {ex.Message}");
        //    }
        //}

        /// <summary>
        /// Postavlja ili uklanja neograničen datum ValidTo.
        /// </summary>
        /// <param name="e">Event sa novom vrijednošću.</param>
        private void ToggleUnlimitedValidTo(ChangeEventArgs e)
        {
            try
            {
                _isUnlimitedValidTo = (bool)e.Value;
                if (ModalMode == ModalMode.INSERT)
                    _createModel.ValidTo = _isUnlimitedValidTo ? null : _createModel.ValidTo;
                else if (ModalMode == ModalMode.EDIT)
                    _updateModel.ValidTo = _isUnlimitedValidTo ? null : _updateModel.ValidTo;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom promjene ograničenja datuma: {ex.Message}");
            }
        }
    }
}
