using AutoDocFront.Components.Modals;
using AutoDocFront.Models.DTO;
using AutoDocFront.Models.DTO.GroupSection;
using AutoDocFront.Models.DTO.Sections;
using AutoDocFront.Models.Enumerations;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.ComponentModel;
using System.Reflection;

namespace AutoDocFront.Components.Pages
{
    public partial class SectionPage
    {
        [Parameter] public int groupId { get; set; }

        private HttpClient _autoDocServiceClient;
        private List<SectionsGetDTO> sections = new();
        private SectionsGetDTO selectedSection;
        private SectionGroupGetDTO group;
        private bool isSearchVisible = false;
        private string searchTerm;
        private bool _loading = false;
        private int _currentOffset = 0;
        private int _pageSize = 50; // Podesiti po potrebi

        private bool isBasicDetailsVisible = true;
        private bool isContentVisible = true;
        private SectionsModal _sectionModal = null!;
        private bool isStatusFilterVisible = false;
        private bool _isSectionModalVisible = false; // Modal visibility flag

        private ModalMode sectionModalMode = ModalMode.VIEW;

        private HashSet<SectionStatusType> selectedStatuses = new()
        {
            SectionStatusType.ACTIVE
        };

        private IEnumerable<SectionsGetDTO> filteredSections => sections
          .Where(g => string.IsNullOrEmpty(searchTerm)
                      || g.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
          .ToList();

        protected override async Task OnInitializedAsync()
        {
            _autoDocServiceClient = httpClientFactory.CreateClient("AutoDocService");
            await LoadGroupDataAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("setSectionHeight");
            }
        }

        private async Task LoadGroupDataAsync()
        {
            try
            {
                _loading = true;

                await LoadGroupSectionsAsync();
                await LoadSectionsAsync();
            }
            catch (HttpRequestException)
            {
                // Handle exception (e.g., show a toast message)
            }
            finally
            {
                _loading = false;
            }
        }

        private async Task LoadSectionsAsync()
        {
            try
            {
                List<SectionsGetDTO> allSections = new List<SectionsGetDTO>();
                PagedList<SectionsGetDTO>? pagedList;
                sections?.Clear();
                selectedSection = null;
                do
                {
                    string url = $"/api/contract-generation/sections?groupId={groupId}&isLatestOnly=true";

                    if (selectedStatuses != null && selectedStatuses.Count == 1)
                    {
                        bool? isActiveSection = null;
                        if (selectedStatuses.FirstOrDefault() == SectionStatusType.ACTIVE)
                        {
                            isActiveSection = true;
                        }
                        else
                        {
                            isActiveSection = false;
                        }
                        url += $"&isActive={isActiveSection.Value}";
                    }

                    url += $"&offset={_currentOffset}&pageSize={_pageSize}";

                    //var statuses = string.Join("&statuses=", selectedStatuses.Select(s => s.ToString()));
                    var response = await _autoDocServiceClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        pagedList = await response.Content.ReadFromJsonAsync<PagedList<SectionsGetDTO>>();
                        if (pagedList?.Items != null)
                        {
                            allSections.AddRange(pagedList.Items);
                            _currentOffset = pagedList.NextPageOffset ?? 0;  // Move to the next offset
                        }
                        else
                        {
                            break;  // No more items to load
                        }
                    }
                    else
                    {
                        // Handle failure
                        toastService.ShowError("Problem prilikom dobavljanja clanova/sekcija!");
                        break;
                    }
                } while (pagedList?.NextPageOffset != null);

                sections = allSections;
            }
            catch (HttpRequestException ex)
            {
                // Handle exception
            }
        }

        public async Task LoadGroupSectionsAsync()
        {
            try
            {
                var response = await _autoDocServiceClient.GetAsync($"/api/contract-generation/section-groups?status=ACTIVE&id={groupId}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<SectionGroupGetDTO>>() ?? new List<SectionGroupGetDTO>();
                    group = result.FirstOrDefault() ?? new SectionGroupGetDTO();
                }
                else if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
                {
                    toastService.ShowError("Problem prilikom dobavljanja podataka za grupe clanova!");
                }
            }
            catch (HttpRequestException ex)
            {
                // TODO: Handle exception
            }
        }

        private void ToggleVisibility(ref bool visibilityFlag)
        {
            visibilityFlag = !visibilityFlag;
        }

        private void ToggleStatusFilter()
        {
            isStatusFilterVisible = !isStatusFilterVisible;
        }

        private async Task OnStatusFilterChanged(ChangeEventArgs e, SectionStatusType status)
        {
            var isChecked = (e.Value as bool?) ?? false;
            if (isChecked)
            {
                selectedStatuses.Add(status);
            }
            else
            {
                selectedStatuses.Remove(status);
            }
            await LoadSectionsAsync();
        }

        private void OpenNewSection()
        {
            NavigationManager.NavigateTo($"/sections/{groupId}/create-section");
        }

        /// <summary>
        /// Metoda koja je kreirana da otvori modal u EDIT modu i da proslijedi odabranu sekciju
        /// </summary>
        private void OpenEditSectionModal()
        {
            sectionModalMode = ModalMode.EDIT;
            _isSectionModalVisible = true;
        }

        /// <summary>
        /// Metoda koja je kreirana da otvori modal u VIEW modu i da proslijedi odabranu sekciju
        /// </summary>
        private void OpenHistoricalSectionModal()
        {
            sectionModalMode = ModalMode.VIEW;
            _isSectionModalVisible = true;
        }

        /// <summary>
        /// Metoda koja sluzi da otvori SectionModal
        /// </summary>
        private void ShowSectionModalForInsert()
        {
            selectedSection = null;
            _isSectionModalVisible = true;
            sectionModalMode = ModalMode.INSERT;
        }

        private void ClosePrompt()
        {
            NavigationManager.NavigateTo("/sections");
        }

        private string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        private void SelectSection(SectionsGetDTO section)
        {
            selectedSection = section;
        }

        private async Task OnSaveAsync()
        {
            await LoadSectionsAsync();
        }

        //private class StatusFilter
        //{
        //    public string Name { get; set; }
        //    public bool IsSelected { get; set; }
        //}


    }
}