using AutoDocFront.Models.DTO;
using AutoDocFront.Models.DTO.GroupSection;
using AutoDocFront.Models.DTO.Sections;
using AutoDocFront.Models.Enumerations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Net.Http;

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
        [Parameter] public SectionGroupUpsertDTO Group { get; set; }

        /// <summary>
        /// Event koji se poziva nakon uspješnog snimanja.
        /// </summary>
        [Parameter] public EventCallback OnSave { get; set; }

        // --- INJECTION ---

        [Inject] private IHttpClientFactory HttpClientFactory { get; set; }
        [Inject] private IToastService ToastService { get; set; }
        [Inject] private IDialogService DialogService { get; set; }

        // --- PRIVATNA POLJA ---

        private HttpClient _autoDocServiceClient;
        private SectionGroupUpsertDTO _model = new();
        private EditContext _editContext;
        private ValidationMessageStore _validationMessageStore;
        private bool _loading = false;

        /// <summary>
        /// Vraća true ako je modal u režimu izmjene (edit).
        /// </summary>
        private bool IsEditMode => Group != null && Group.ID > 0;

        /// <summary>
        /// Inicijalizacija komponente
        /// </summary>
        protected override void OnInitialized()
        {
            _autoDocServiceClient = HttpClientFactory.CreateClient("AutoDocService");
        }

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
        private async Task CloseModal()
        {
            IsOpen = false;
            await IsOpenChanged.InvokeAsync(false);
        }

        /// <summary>
        /// Validira i šalje podatke za unos ili izmjenu grupe.
        /// </summary>
        private async Task HandleValidSubmit()
        {
            if (!ValidateForm() || _loading)
                return;

            _loading = true;
            try
            {
                _model.User = "zlatan.kahriman";
                var response = IsEditMode
                    ? await _autoDocServiceClient.PutAsJsonAsync("/api/contract-generation/section-groups", _model)
                    : await _autoDocServiceClient.PostAsJsonAsync("/api/contract-generation/section-groups", _model);

                if (response.IsSuccessStatusCode)
                {
                    ToastService.ShowSuccess(IsEditMode ? "Grupa je uspješno izmijenjena!" : "Grupa je uspješno kreirana!");
                    await CloseModal();
                    await OnSave.InvokeAsync();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    ToastService.ShowError($"Greška: {error}");
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
            if (_loading) return;
            try
            {
                _loading = true;
                if (newStatus == GroupStatusType.DEACTIVATED)
                {
                    var response = await _autoDocServiceClient.GetAsync($"/api/contract-generation/sections?groupId={_model.ID}&isActive=true&pageSize=1");
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadFromJsonAsync<PagedList<SectionsGetDTO>>();
                        if (result != null && result.Items.Any())
                        {
                            await ShowWarningAsync();
                            return;
                        }
                    }
                }
                _model.Status = newStatus;
                await HandleValidSubmit();
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
        /// Prikazuje upozorenje ako grupa ima aktivne sekcije.
        /// </summary>
        private async Task ShowWarningAsync()
        {
            await DialogService.ShowWarningAsync("Nije moguće deaktivirati grupu dok postoji aktivan član (sekcija) u grupi!");
        }
    }
}
