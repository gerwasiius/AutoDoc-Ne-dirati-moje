using AutoDoc.Shared.Model.DTO.SectionGroupDTO;
using AutoDoc.Shared.Model.DTO.SectionsDTO;
using AutoDocFront.Components.Shared;
using AutoDocFront.Models.Enumerations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Net;
using System.Net.Http.Json;

namespace AutoDocFront.Components.Modals
{
    /// <summary>
    /// Modal komponenta za unos, izmjenu i pregled sekcija (članova) unutar grupe.
    /// Omogućava validaciju, prikaz svih verzija i promjenu statusa sekcije.
    /// </summary>
    public partial class SectionsModal
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
        /// DTO objekat grupe kojoj sekcija pripada.
        /// </summary>
        [Parameter] public SectionGroupGetDTO Group { get; set; }

        /// <summary>
        /// DTO objekat sekcije za prikaz ili izmjenu.
        /// </summary>
        [Parameter] public SectionsGetDTO Section { get; set; }

        /// <summary>
        /// Režim rada modala (unos, izmjena, pregled).
        /// </summary>
        [Parameter] public ModalMode ModalMode { get; set; }

        /// <summary>
        /// Event koji se poziva nakon uspješnog snimanja.
        /// </summary>
        [Parameter] public EventCallback OnSave { get; set; }

        // --- INJECTION ---

        /// <summary>
        /// Servis za rad sa sekcijama.
        /// </summary>
        [Inject] private SectionsApiService SectionsService { get; set; } = default!;

        /// <summary>
        /// Servis za prikaz notifikacija (toast poruka).
        /// </summary>
        [Inject] private IToastService ToastService { get; set; }
        [Inject] private IDialogService DialogService{ get; set; }

        /// <summary>
        /// Servis za prikaz dijaloga.
        /// </summary>
        //[Inject] private IDialogService DialogService { get; set; }

        // --- PRIVATNA POLJA ---

        private SectionsCreateDTO _createModel;
        private SectionsUpdateDTO _updateModel;
        private SectionsGetDTO _viewModel;
        private EditContext _editContext;
        private ValidationMessageStore _validationMessageStore;

        private List<SectionsGetDTO> _listSections;
        private TinyMCE _tinyMceEditor;

        private bool _isLoading;
        private string _modalStyle => IsOpen ? "display: block;" : "display: none;";

        // --- LIFECYCLE ---
        /// <summary>
        /// Inicijalizuje modal, priprema model i učitava verzije sekcije ako je u VIEW modu.
        /// </summary>
        protected override async Task OnParametersSetAsync()
        {
            try
            {
                switch (ModalMode)
                {
                    case ModalMode.INSERT:
                        _createModel = new SectionsCreateDTO
                        {
                            UserInserted = "zlatan.kahriman",
                            IsActive = true,
                            GroupId = Group.ID
                        };
                        _editContext = new EditContext(_createModel);
                        break;
                    case ModalMode.EDIT:
                        _updateModel = new SectionsUpdateDTO
                        {
                            GroupId = Section.GroupId,
                            Name = Section.Name,
                            Description = Section.Description,
                            Content = Section.Content,
                            IsActive = Section.IsActive,
                            UserUpdated = "zlatan.kahriman"
                        };
                        _editContext = new EditContext(_updateModel);
                        break;
                    case ModalMode.VIEW:
                        _viewModel = Section;
                        break;
                }
                if (ModalMode != ModalMode.VIEW)
                    _validationMessageStore = new ValidationMessageStore(_editContext);

                if (ModalMode == ModalMode.VIEW)
                {
                    await LoadAllVersionsForSectionAsync();
                }
            }
            catch (Exception ex)
            {
                await DialogService.ShowErrorAsync("Došlo je do greške prilikom inicijalizacije modala.\n\nDetalji: " + ex.Message);
            }
        }

        // --- METODE ---

        // Zatvaranje modala implementirano u baznoj klasi.

        /// <summary>
        /// Validira formu i izvršava submit (insert ili update sekcije).
        /// </summary>
        private async Task HandleValidSubmitAsync()
        {
            try
            {
                _validationMessageStore?.Clear();

                // Ažuriraj sadržaj iz TinyMCE editora
                if (_tinyMceEditor != null)
                    await _tinyMceEditor.UpdateContentFromEditor();

                if (_editContext.Validate())
                {
                    if (ModalMode == ModalMode.EDIT)
                        await UpdateSectionAsync();
                    else if (ModalMode == ModalMode.INSERT)
                        await InsertNewSectionAsync();
                }
                else
                {
                    _editContext.NotifyValidationStateChanged();
                    ToastService.ShowError("Potrebno je provjeriti da li su sva polja uredno unesena!");
                }
            }
            catch (Exception ex)
            {
                await DialogService.ShowErrorAsync("Neočekivana greška prilikom validacije ili slanja forme.\n\nDetalji: " + ex.Message);
            }
        }

        /// <summary>
        /// Kreira novu sekciju (prva verzija).
        /// </summary>
        private async Task InsertNewSectionAsync()
        {
            try
            {
                _isLoading = true;
                var result = await SectionsService.InsertSectionAsync(_createModel);

                if (!result.IsSuccess)
                {
                    if (result.StatusCode == HttpStatusCode.Conflict)
                        ToastService.ShowError(result.ErrorMessage ?? "Sekcija sa istim nazivom već postoji.");
                    else
                        await DialogService.ShowErrorAsync(result.ErrorMessage ?? "Problem prilikom upisa sekcije!");
                }
                else
                {
                    ToastService.ShowSuccess("Sekcija je uspješno spašena!");
                    await CloseAsync();
                    await OnSave.InvokeAsync();
                }
            }
            catch (Exception ex)
            {
                await DialogService.ShowErrorAsync("Neočekivana greška prilikom kreiranja sekcije.\n\nDetalji: " + ex.Message);
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// Ažurira postojeću sekciju (kreira novu verziju).
        /// </summary>
        private async Task UpdateSectionAsync()
        {
            try
            {
                _isLoading = true;
                var result = await SectionsService.UpdateSectionAsync(Section.ID, _updateModel);

                if (!result.IsSuccess)
                {
                    await DialogService.ShowErrorAsync(result.ErrorMessage ?? "Problem prilikom ažuriranja sekcije!");
                }
                else
                {
                    ToastService.ShowSuccess("Sekcija je uspješno ažurirana!");
                    await CloseAsync();
                    await OnSave.InvokeAsync();
                }
            }
            catch (Exception ex)
            {
                await DialogService.ShowErrorAsync("Neočekivana greška prilikom ažuriranja sekcije.\n\nDetalji: " + ex.Message);
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// Učitava sve verzije sekcije za prikaz u VIEW modu.
        /// </summary>
        private async Task LoadAllVersionsForSectionAsync()
        {
            try
            {
                if (Section?.IdSection == null)
                    return;

                var result = await SectionsService.GetAllVersionsForSectionAsync(Section.IdSection.Value);

                _listSections = result?.OrderByDescending(e => e.Version).ToList() ?? new List<SectionsGetDTO>();
            }
            catch (Exception ex)
            {
                await DialogService.ShowErrorAsync("Problem prilikom dobavljanja ostalih verzija sekcije.\n\nDetalji: " + ex.Message);
            }
        }

        /// <summary>
        /// Odabire verziju sekcije u VIEW modu i popunjava formu.
        /// </summary>
        /// <param name="section">Odabrana verzija sekcije.</param>
        private void SelectVersion(SectionsGetDTO section)
        {
            _viewModel = section;
            StateHasChanged();
        }

        /// <summary>
        /// Aktivira ili deaktivira sekciju.
        /// </summary>
        /// <param name="isActive">True za aktivaciju, False za deaktivaciju.</param>
        private async Task ToggleSectionStatusAsync(bool isActive)
        {
            try
            {
                _isLoading = true;
                var success = await SectionsService.UpdateSectionStatusAsync(Section.ID, Section.IdSection, isActive);
                if (!success)
                {
                    ToastService.ShowError(isActive ? "Problem prilikom aktivacije sekcije!" : "Problem prilikom deaktivacije sekcije!");
                }
                else
                {
                    ToastService.ShowSuccess(isActive ? "Sekcija je uspješno aktivirana!" : "Sekcija je uspješno deaktivirana!");
                    await CloseAsync();
                    await OnSave.InvokeAsync();
                }
            }
            catch (Exception ex)
            {
                await DialogService.ShowErrorAsync("Neočekivana greška prilikom promjene statusa sekcije.\n\nDetalji: " + ex.Message);
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// Prikazuje dijalog za potvrdu aktivacije ili deaktivacije sekcije.
        /// </summary>
        /// <param name="isActive">True za aktivaciju, False za deaktivaciju.</param>
        private async Task ShowConfirmationDialogAsync(bool isActive)
        {
                await ToggleSectionStatusAsync(isActive);
        }
        /// <summary>
        /// Zatvara modal i emituje promjenu stanja.
        /// </summary>
        private async Task CloseAsync()
        {
            IsOpen = false;
            await IsOpenChanged.InvokeAsync(false);
        }
    }
}