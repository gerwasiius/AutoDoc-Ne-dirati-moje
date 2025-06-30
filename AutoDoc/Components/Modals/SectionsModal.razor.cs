using AutoDocFront.Components.Shared;
using AutoDocFront.Models.DTO;
using AutoDocFront.Models.DTO.GroupSection;
using AutoDocFront.Models.DTO.Sections;
using AutoDocFront.Models.Enumerations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Net;
using System.Net.Http.Json;
using Icons = Microsoft.FluentUI.AspNetCore.Components.Icons;

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

        /// <summary>
        /// Servis za prikaz dijaloga.
        /// </summary>
        [Inject] private IDialogService DialogService { get; set; }

        // --- PRIVATNA POLJA ---

        private HttpClient _client;
        private EditContext _editContext;
        private ValidationMessageStore _validationMessageStore;
        private SectionsGetDTO _model;
        private List<SectionsGetDTO> _listSections;
        private bool _loading = false;
        private TinyMCE _tinyMceEditor;
        private string _modalStyle => IsOpen ? "display: block;" : "display: none;";

        // --- LIFECYCLE ---
        /// <summary>
        /// Inicijalizuje modal, priprema model i učitava verzije sekcije ako je u VIEW modu.
        /// </summary>
        protected override async Task OnParametersSetAsync()
        {
            _model = new SectionsGetDTO();

            switch (ModalMode)
            {
                case ModalMode.INSERT:
                    _model.UserInsert = "zlatan.kahriman"; // TODO: Zamijeniti sa stvarnim korisnikom
                    _model.IsActive = true;
                    break;
                case ModalMode.EDIT:
                case ModalMode.VIEW:
                    _model = Section;
                    break;
            }

            _editContext = new EditContext(_model);
            _validationMessageStore = new ValidationMessageStore(_editContext);

            if (ModalMode == ModalMode.VIEW)
            {
                await LoadAllVersionsForSectionAsync();
            }
        }

        // --- METODE ---

        /// <summary>
        /// Zatvara modal i emituje promjenu stanja.
        /// </summary>
        private async Task CloseModalAsync()
        {
            IsOpen = false;
            await IsOpenChanged.InvokeAsync(false);
        }

        /// <summary>
        /// Validira formu i izvršava submit (insert ili update sekcije).
        /// </summary>
        private async Task HandleValidSubmitAsync()
        {
            _validationMessageStore.Clear();

            // Ažuriraj sadržaj iz TinyMCE editora
            if (_tinyMceEditor != null)
            {
                await _tinyMceEditor.UpdateContentFromEditor();
            }

            if (_editContext.Validate())
            {
                if (ModalMode == ModalMode.EDIT)
                {
                    await UpdateSectionAsync();
                }
                else if (ModalMode == ModalMode.INSERT)
                {
                    _model.GroupId = Group.ID;
                    await InsertNewSectionAsync();
                }
            }
            else
            {
                _editContext.NotifyValidationStateChanged();
                ToastService.ShowError("Potrebno je provjeriti da li su sva polja uredno unesena!");
            }
        }

        /// <summary>
        /// Kreira novu sekciju (prva verzija).
        /// </summary>
        private async Task InsertNewSectionAsync()
        {
            try
            {
                _loading = true;

                var createDTO = new SectionsCreateDTO
                {
                    GroupId = _model.GroupId,
                    Name = _model.Name,
                    Description = _model.Description,
                    Content = _model.Content,
                    IsActive = _model.IsActive,
                    UserInsert = _model.UserInsert
                };

                var result = await SectionsService.InsertSectionAsync(createDTO);

                if (!result.IsSuccess)
                {
                    if (result.StatusCode == HttpStatusCode.Conflict)
                        ToastService.ShowError(result.ErrorMessage ?? "Sekcija sa istim nazivom već postoji.");
                    else
                        ToastService.ShowError("Problem prilikom upisa sekcije!");
                }
                else
                {
                    ToastService.ShowSuccess("Sekcija je uspješno spašena!");
                    await CloseModalAsync();
                    await OnSave.InvokeAsync();
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Neočekivana greška: {ex.Message}");
            }
            finally
            {
                _loading = false;
            }
        }

        /// <summary>
        /// Ažurira postojeću sekciju (kreira novu verziju).
        /// </summary>
        private async Task UpdateSectionAsync()
        {
            try
            {
                _loading = true;

                var updateDTO = new SectionsUpdateDTO
                {
                    GroupId = _model.GroupId,
                    Name = _model.Name,
                    Description = _model.Description,
                    Content = _model.Content,
                    IsActive = _model.IsActive,
                    UserUpdate = "zlatan.kahriman" // TODO: Zamijeniti sa stvarnim korisnikom
                };

                var result = await SectionsService.UpdateSectionAsync(_model.ID, updateDTO);

                if (!result.IsSuccess)
                {
                    ToastService.ShowError(result.ErrorMessage ?? "Problem prilikom ažuriranja sekcije!");
                }
                else
                {
                    ToastService.ShowSuccess("Sekcija je uspješno ažurirana!");
                    await CloseModalAsync();
                    await OnSave.InvokeAsync();
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Neočekivana greška: {ex.Message}");
            }
            finally
            {
                _loading = false;
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
                ToastService.ShowError($"Problem prilikom dobavljanja ostalih verzija sekcije: {ex.Message}");
            }
        }

        /// <summary>
        /// Odabire verziju sekcije u VIEW modu i popunjava formu.
        /// </summary>
        /// <param name="section">Odabrana verzija sekcije.</param>
        private void SelectVersion(SectionsGetDTO section)
        {
            _model = section;
            _editContext = new EditContext(_model);
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
                _loading = true;
                var success = await SectionsService.UpdateSectionStatusAsync(_model.ID, _model.IdSection, isActive);
                if (!success)
                {
                    ToastService.ShowError(isActive ? "Problem prilikom aktivacije sekcije!" : "Problem prilikom deaktivacije sekcije!");
                }
                else
                {
                    ToastService.ShowSuccess(isActive ? "Sekcija je uspješno aktivirana!" : "Sekcija je uspješno deaktivirana!");
                    await CloseModalAsync();
                    await OnSave.InvokeAsync();
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Neočekivana greška: {ex.Message}");
            }
            finally
            {
                _loading = false;
            }
        }

        /// <summary>
        /// Prikazuje dijalog za potvrdu aktivacije ili deaktivacije sekcije.
        /// </summary>
        /// <param name="isActive">True za aktivaciju, False za deaktivaciju.</param>
        private async Task ShowConfirmationDialogAsync(bool isActive)
        {
            var action = isActive ? "aktivirati" : "deaktivirati";

            var dialog = await DialogService.ShowMessageBoxAsync(new DialogParameters<MessageBoxContent>
            {
                Content = new()
                {
                    Title = "Da li ste sigurni?",
                    MarkupMessage = new MarkupString($"Da li ste sigurni da želite <b>{action}</b> sekciju?"),
                    Icon = new Icons.Regular.Size24.QuestionCircle(),
                    IconColor = isActive ? Color.Success : Color.Error,
                },
                Modal = true,
                TrapFocus = true,
                PrimaryAction = "DA",
                SecondaryAction = "NE",
                Width = "400px",
            });

            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                await ToggleSectionStatusAsync(isActive);
            }
        }
    }
}