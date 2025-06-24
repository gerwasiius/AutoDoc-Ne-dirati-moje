using AutoDocFront.Components.Shared;
using AutoDocFront.Models;
using AutoDocFront.Models.DTO.GroupSection;
using AutoDocFront.Models.DTO.Sections;
using AutoDocFront.Models.Enumerations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AutoDocFront.Components.Pages
{
    public partial class SectionCreate2
    {
        [Parameter] public int groupId { get; set; }

        private SectionGroupGetDTO group;
        private SectionsUpsertDTO _model = new();
        private EditContext _editContext;
        private ValidationMessageStore _validationMessageStore;
        private HttpClient _autoDocServiceClient;
        private TinyMCE tinyMceEditor;

        private Parametri _parametri = new Parametri();
        private Dictionary<string, Dictionary<string, string>> _placeholders;

        private enum ButtonState { Button1, Button2 }
        private ButtonState activeButton = ButtonState.Button1;
        private string activeTab = "details"; // Default active tab


        protected override async Task OnInitializedAsync()
        {
            _autoDocServiceClient = httpClientFactory.CreateClient("AutoDocService");
            await LoadGroupSectionsAsync();
            _editContext = new EditContext(_model);
            _validationMessageStore = new ValidationMessageStore(_editContext);

            _placeholders = GetPlaceholders(_parametri);
        }

        public async Task LoadGroupSectionsAsync()
        {
            try
            {
                var response = await _autoDocServiceClient.GetAsync($"/api/ContractGeneration/SectionGroup?id={groupId}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<SectionGroupGetDTO>>() ?? new List<SectionGroupGetDTO>();
                    group = result.FirstOrDefault() ?? new SectionGroupGetDTO();
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
        }

        private void ClosePrompt()
        {
            NavigationManager.NavigateTo($"/sections/{groupId}");
        }

        private async Task HandleValidSubmit()
        {
            await tinyMceEditor.UpdateContentFromEditor();

            _validationMessageStore.Clear();
            _model.GroupId = groupId;
            if (_editContext.Validate())
            {
                await InsertSectionGroup(_model);
                toastService.ShowSuccess("Novi clan je uspjesno upisan!");
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
                model.GroupId = groupId;
                model.User = "zlatan.kahriman"; // TO DO - Replace with actual user
                //model.Status = SectionStatusType.ACTIVE;

                var response = await _autoDocServiceClient.PostAsJsonAsync("/api/ContractGeneration/Sections", model);

                if (!response.IsSuccessStatusCode)
                {
                    toastService.ShowError("Problem prilikom upisa grupe!");
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Unexpected error: {ex.Message}");
            }
        }

        private async Task HandleBackToSection()
        {
            NavigationManager.NavigateTo("/sections");
        }

        //DRUGA VERZIJA
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

        private void SelectTab(string tabName)
        {
            activeTab = tabName;
        }

        private string addClassToActiveTab(string tabName)
        {
            return activeTab == tabName ? "active" : string.Empty;
        }


        private void SetActiveButton(ButtonState button)
        {
            activeButton = button;
        }

        private string GetButtonClass(ButtonState button)
        {
            return activeButton == button ? "btn-light" : "btn-transparent";
        }
    }
}