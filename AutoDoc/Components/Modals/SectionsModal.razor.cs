using AutoDocFront.Components.Shared;
using AutoDocFront.Models.DTO;
using AutoDocFront.Models.DTO.GroupSection;
using AutoDocFront.Models.DTO.Sections;
using AutoDocFront.Models.Enumerations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;
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
        protected override async Task OnInitializedAsync()
        {
            _client = HttpClientFactory.CreateClient("AutoDocService");
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
                await LoadAllVersionsForSection();
            }
        }

        // --- METODE ---

        /// <summary>
        /// Zatvara modal i emituje promjenu stanja.
        /// </summary>
        private void CloseModal()
        {
            IsOpen = false;
            IsOpenChanged.InvokeAsync(false);
        }

        /// <summary>
        /// Validira formu i izvršava submit (insert ili update sekcije).
        /// </summary>
        private async Task HandleValidSubmit()
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
                    await UpdateSection();
                }
                else if (ModalMode == ModalMode.INSERT)
                {
                    _model.GroupId = Group.ID;
                    await InsertNewSection();
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
        private async Task InsertNewSection()
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

                var response = await _client.PostAsJsonAsync("/api/contract-generation/sections", createDTO);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        ToastService.ShowError(errorMessage);
                    }
                    else
                    {
                        ToastService.ShowError("Problem prilikom upisa člana!");
                    }
                }
                else
                {
                    ToastService.ShowSuccess("Član je uspješno spašen!");
                    CloseModal();
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
        private async Task UpdateSection()
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

                var response = await _client.PutAsJsonAsync($"/api/contract-generation/sections/{Section.ID}/manage-section", updateDTO);

                if (!response.IsSuccessStatusCode)
                {
                    ToastService.ShowError("Problem prilikom ažuriranja člana!");
                }
                else
                {
                    ToastService.ShowSuccess("Član je uspješno ažuriran!");
                    CloseModal();
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
        private async Task LoadAllVersionsForSection()
        {
            try
            {
                var response = await _client.GetAsync($"/api/contract-generation/sections?idSection={Section.IdSection}&offset=0&pageSize=0");
                if (response.IsSuccessStatusCode)
                {
                    _listSections = (await response.Content.ReadFromJsonAsync<PagedList<SectionsGetDTO>>())?.Items
                        ?.OrderByDescending(e => e.Version)
                        .ToList();
                }
                else
                {
                    ToastService.ShowError("Problem prilikom dobavljanja ostalih verzija člana/sekcije!");
                }
            }
            catch (HttpRequestException)
            {
                // Dodatna obrada greške po potrebi
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
        private async Task ToggleSectionStatus(bool isActive)
        {
            try
            {
                _loading = true;

                var statusUpdateDTO = new { IsActive = isActive };

                var response = await _client.PatchAsJsonAsync(
                    $"/api/contract-generation/sections/update-status?sectionId={Section.IdSection}&isActiveStatus={isActive}",
                    statusUpdateDTO);

                if (!response.IsSuccessStatusCode)
                {
                    ToastService.ShowError(isActive ? "Problem prilikom aktivacije člana!" : "Problem prilikom deaktivacije člana!");
                }
                else
                {
                    ToastService.ShowSuccess(isActive ? "Član je uspješno aktiviran!" : "Član je uspješno deaktiviran!");
                    CloseModal();
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
        private async Task ShowConfirmationDialog(bool isActive)
        {
            var action = isActive ? "aktivirati" : "deaktivirati";

            var dialog = await DialogService.ShowMessageBoxAsync(new DialogParameters<MessageBoxContent>
            {
                Content = new()
                {
                    Title = "Da li ste sigurni?",
                    MarkupMessage = new MarkupString($"Da li ste sigurni da želite <b>{action}</b> član?"),
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
                await ToggleSectionStatus(isActive);
            }
        }
    }
}