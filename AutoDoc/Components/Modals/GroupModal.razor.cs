using AutoDoc.Shared.Model.DTO.Enumerations;
using AutoDoc.Shared.Model.DTO.SectionGroupDTO;
using AutoDocFront.Components.Shared;
using AutoDocFront.Models.Enumerations;
using AutoDocFront.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;

namespace AutoDocFront.Components.Modals
{
    /// <summary>
    /// Komponenta za prikaz i upravljanje modalom za unos ili izmjenu grupe sekcija.
    /// </summary>
    public partial class GroupModal
    {
        // --- PARAMETRI ---

        /// <summary>
        /// Označava da li je modal otvoren.
        /// </summary>
        [Parameter] public bool IsOpen { get; set; }

        /// <summary>
        /// Event za promjenu stanja otvaranja modala.
        /// </summary>
        [Parameter] public EventCallback<bool> IsOpenChanged { get; set; }

        /// <summary>
        /// DTO objekat grupe za izmjenu (null za unos nove grupe).
        /// </summary>
        [Parameter] public SectionGroupUpsertDTO? Group { get; set; }

        /// <summary>
        /// Event koji se poziva nakon uspješnog snimanja.
        /// </summary>
        [Parameter] public EventCallback OnSave { get; set; }
        /// <summary>
        /// Oznaka kroz koji mod ce se otvoriti forma
        /// </summary>
        [Parameter] public ModalMode Mode { get; set; }


        // --- INJECTION ---

        [Inject] private SectionGroupApiService GroupService { get; set; }
        [Inject] private IToastService ToastService { get; set; }
        [Inject] private IDialogService DialogService { get; set; }

        // --- PRIVATNA POLJA ---

        private SectionGroupUpsertDTO _model = new();
        private EditContext _editContext;
        private ValidationMessageStore _validationMessageStore;
        private bool _isLoading = false;

        /// <summary>
        /// Vraća true ako je modal u režimu izmjene (edit).
        /// </summary>
        private bool IsEditMode => Group != null && Group.ID > 0;

        /// <summary>
        /// On parameter set model i edit kontekst na osnovu proslijeđenih parametara.
        /// </summary>
        protected override void OnParametersSet()
        {
            _model = Group != null
                ? new SectionGroupUpsertDTO
                {
                    ID = Group.ID,
                    Name = Group.Name,
                    Description = Group.Description,
                    Status = Group.Status
                }
                : new SectionGroupUpsertDTO { Status = GroupStatusType.ACTIVE };

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
        /// Validira i šalje podatke za unos ili izmjenu grupe.
        /// </summary>
        private async Task HandleValidSubmitAsync()
        {
            if (_isLoading || !ValidateForm())
                return;

            _isLoading = true;
            try
            {
                _model.User = "zlatan.kahriman";
                bool success = IsEditMode
    ? await GroupService.UpdateGroupAsync(new SectionGroupUpdateDTO
    {
        ID = _model.ID ?? 0,
        Name = _model.Name,
        Description = _model.Description,
        Status = _model.Status,
        UserUpdated = _model.User
    })
    : await GroupService.CreateGroupAsync(new SectionGroupCreateDTO
    {
        Name = _model.Name,
        Description = _model.Description,
        Status = _model.Status ?? GroupStatusType.ACTIVE,
        UserInserted = _model.User
    });

                if (success)
                {
                    ToastService.ShowSuccess(IsEditMode ? "Grupa je uspješno izmijenjena!" : "Grupa je uspješno kreirana!");
                    await CloseModalAsync();
                    await OnSave.InvokeAsync();
                }
                else
                {
                    ToastService.ShowError("Greška prilikom snimanja grupe.");
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

        /// <summary>
        /// Metoda koja sluzi da validira da li je moguce izvrsiti izmjenu
        /// </summary>
        /// <returns></returns>
        private bool ValidateForm()
        {
            _validationMessageStore.Clear();
            if (!_editContext.Validate())
            {
                _editContext.NotifyValidationStateChanged();
                ToastService.ShowError("Provjerite da li su sva polja ispravno popunjena.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Mijenja status grupe na zadani status. Prije deaktivacije provjerava da li postoje aktivne sekcije.
        /// </summary>
        /// <param name="newStatus">Novi status grupe</param>
        private async Task ChangeGroupStatusAsync(GroupStatusType newStatus)
        {
            if (_isLoading)
                return;

            string action = newStatus == GroupStatusType.ACTIVE ? "aktivirate" : "deaktivirate";
            string message = $"Da li ste sigurni da želite da {action} ovu grupu?";

            //Dodajte dodatno dugme "Otkaži"
            var dialogRef = await DialogService.ShowConfirmationAsync(
                message,
                "Da",
                "Ne",
                "Otkaži" // Ovo je dodatno dugme
            );

            var result = await dialogRef.Result;
            var canceled = result.Cancelled;
            if (canceled)
                return;
            try
            {
                _isLoading = true;
                if (newStatus == GroupStatusType.DEACTIVATED)
                {
                    if (_model.ID.HasValue && await GroupService.HasActiveSectionsAsync(_model.ID.Value))
                    {
                        await ShowWarningAsync();
                        return;
                    }
                }
                _model.Status = newStatus;
                await HandleValidSubmitAsync();
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

        /// <summary>
        /// Prikazuje upozorenje ako grupa ima aktivne sekcije.
        /// </summary>
        private async Task ShowWarningAsync()
        {
            await DialogService.ShowWarningAsync("Nije moguće deaktivirati grupu dok postoji aktivan član (sekcija) u grupi!");
        }
    }
}
