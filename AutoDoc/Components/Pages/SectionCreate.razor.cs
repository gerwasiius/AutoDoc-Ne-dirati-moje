using AutoDocFront.Components.Shared;
using AutoDocFront.Models;
using AutoDocFront.Models.DTO;
using AutoDocFront.Models.DTO.GroupSection;
using AutoDocFront.Models.DTO.Sections;
using AutoDocFront.Models.Enumerations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AutoDocFront.Components.Pages
{
    public partial class SectionCreate
    {
        [Parameter] public int GroupId { get; set; }
        [Parameter] public int? SectionId { get; set; } // Nullable for create mode

        private SectionGroupGetDTO _group;
        private SectionsUpsertDTO _model = new();
        private EditContext _editContext;
        private ValidationMessageStore _validationMessageStore;
        private HttpClient _autoDocServiceClient;
        private TinyMCE tinyMceEditor;
        private bool _loading = false;

        private Parametri _parametri = new Parametri();
        private Dictionary<string, Dictionary<string, string>> _placeholders;
        private bool _isPlaceholderAvailable = false;

        private string activeTab = "details"; // Default active tab
        private bool isEditMode => SectionId.HasValue;

        protected override async Task OnInitializedAsync()
        {
            _autoDocServiceClient = httpClientFactory.CreateClient("AutoDocService");
            await LoadGroupSectionsAsync();
            _editContext = new EditContext(_model);
            _validationMessageStore = new ValidationMessageStore(_editContext);

            _placeholders = GetPlaceholders(_parametri);

            if (isEditMode)
            {
                await LoadSectionAsync();
            }
        }

        private async Task LoadGroupSectionsAsync()
        {
            try
            {
                _loading = true;

                var response = await _autoDocServiceClient.GetAsync($"/api/contract-generation/section-groups?id={GroupId}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<SectionGroupGetDTO>>() ?? new List<SectionGroupGetDTO>();
                    _group = result.FirstOrDefault() ?? new SectionGroupGetDTO();
                }
                else
                {
                    toastService.ShowError("Failed to load group sections!");
                }
            }
            catch (HttpRequestException ex)
            {
                toastService.ShowError($"Unexpected error: {ex.Message}");
            }
            finally
            {
                _loading = false;
            }
        }

        private async Task LoadSectionAsync()
        {
            try
            {
                _loading = true;

                var response = await _autoDocServiceClient.GetAsync($"/api/ContractGeneration/Sections?id={SectionId}");
                if (response.IsSuccessStatusCode)
                {
                    var result = (await response.Content.ReadFromJsonAsync<PagedList<SectionsGetDTO>>()).Items.FirstOrDefault();
                    if (result != null)
                    {
                        _model = new SectionsUpsertDTO
                        {
                            //ID = result.ID,
                            GroupId = result.GroupId,
                            Name = result.Name,
                            Description = result.Description,
                            Content = result.Content,
                            //Status = SectionStatusType.ACTIVE,
                            User = "zlatan.kahriman"
                        };

                        _editContext = new EditContext(_model);
                        _validationMessageStore = new ValidationMessageStore(_editContext);
                    }
                }
                else
                {
                    toastService.ShowError("Failed to load section!");
                }
            }
            catch (HttpRequestException ex)
            {
                toastService.ShowError($"Unexpected error: {ex.Message}");
            }
            finally
            {
                _loading = false;
            }
        }

        private void ClosePrompt()
        {
            NavigationManager.NavigateTo($"/sections/{GroupId}");
        }

        private async Task HandleValidSubmit()
        {
            if (activeTab == "editor" && tinyMceEditor != null)
            {
                await tinyMceEditor.UpdateContentFromEditor();
            }

            _validationMessageStore.Clear();
            _model.GroupId = GroupId;
            if (_editContext.Validate())
            {
                if (isEditMode)
                {
                    await UpdateSectionGroup(_model);
                    toastService.ShowSuccess("Član je uspješno ažuriran!");
                }
                else
                {
                    await InsertSectionGroup(_model);
                    toastService.ShowSuccess("Novi član je uspješno upisan!");
                }

                ClosePrompt();
            }
            else
            {
                _editContext.NotifyValidationStateChanged();
            }
        }

        private async Task InsertSectionGroup(SectionsUpsertDTO model)
        {
            try
            {
                _loading = true;

                model.User = "zlatan.kahriman"; // TO DO - Replace with actual user
                //model.Status = SectionStatusType.ACTIVE;

                var response = await _autoDocServiceClient.PostAsJsonAsync("/api/ContractGeneration/Sections", model);

                if (!response.IsSuccessStatusCode)
                {
                    toastService.ShowError("Problem prilikom upisa člana!");
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Unexpected error: {ex.Message}");
            }
            finally
            {
                _loading = false;
            }
        }

        private async Task UpdateSectionGroup(SectionsUpsertDTO model)
        {
            try
            {
                _loading = true;

                var response = await _autoDocServiceClient.PutAsJsonAsync($"/api/ContractGeneration/Sections", model);

                if (!response.IsSuccessStatusCode)
                {
                    toastService.ShowError("Problem prilikom ažuriranja člana!");
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Unexpected error: {ex.Message}");
            }
            finally
            {
                _loading = false;
            }
        }

        private Dictionary<string, Dictionary<string, string>> GetPlaceholders(object obj, string parentKey = "")
        {
            var placeholders = new Dictionary<string, Dictionary<string, string>>();
            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                var displayName = prop.GetCustomAttribute<DisplayAttribute>()?.Name ?? prop.Name;
                var key = string.IsNullOrEmpty(parentKey) ? prop.Name : $"{parentKey}_{prop.Name}";

                if (prop.PropertyType.IsClass && !prop.PropertyType.IsPrimitive && prop.PropertyType != typeof(string))
                {
                    var nestedPlaceholders = GetPlaceholders(prop.GetValue(obj), key);
                    placeholders[prop.Name] = nestedPlaceholders.SelectMany(np => np.Value).ToDictionary(np => np.Key, np => np.Value);
                }
                else
                {
                    if (placeholders.ContainsKey(parentKey))
                    {
                        placeholders[parentKey].Add(key, displayName);
                    }
                    else
                    {
                        placeholders[parentKey] = new Dictionary<string, string> { { key, displayName } };
                    }
                }
            }

            return placeholders;
        }

        private async Task SelectTab(string tabName)
        {
            if (activeTab == "editor" && tabName == "details" && tinyMceEditor != null)
            {
                await tinyMceEditor.UpdateContentFromEditor();
            }

            activeTab = tabName;
            _isPlaceholderAvailable = tabName == "editor";
        }

        private string addClassToActiveTab(string tabName)
        {
            return activeTab == tabName ? "active" : string.Empty;
        }
    }
}
