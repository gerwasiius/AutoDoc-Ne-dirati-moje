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

        [Inject] private SectionsApiService SectionsService { get; set; } = default!;
        [Inject] private SectionGroupApiService GroupService { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private IToastService ToastService { get; set; } = default!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

        private List<SectionsGetDTO> _sections = new();
        private SectionsGetDTO? _selectedSection;
        private SectionGroupGetDTO? _group;
        private bool _isSearchVisible = false;
        private string _searchTerm = string.Empty;
        private bool _loading = false;
        private int _currentOffset = 0;
        private int _pageSize = 50; // Podesiti po potrebi

        private bool _isBasicDetailsVisible = true;
        private bool _isContentVisible = true;
        private SectionsModal _sectionModal = null!;
        private bool _isStatusFilterVisible = false;
        private bool _isSectionModalVisible = false; // Modal visibility flag

        private ModalMode _sectionModalMode = ModalMode.VIEW;

        private HashSet<SectionStatusType> _selectedStatuses = new()
        {
            SectionStatusType.ACTIVE
        };

        private IEnumerable<SectionsGetDTO> FilteredSections => _sections
          .Where(g => string.IsNullOrEmpty(_searchTerm)
                      || g.Name.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase))
          .ToList();

        protected override async Task OnInitializedAsync()
        {
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
                var allSections = new List<SectionsGetDTO>();
                PagedList<SectionsGetDTO>? pagedList;
                _sections.Clear();
                _selectedSection = null;

                do
                {
                    SectionStatusType? status = null;
                    if (_selectedStatuses.Count == 1)
                    {
                        status = _selectedStatuses.First();
                    }

                    pagedList = await SectionsService.GetSectionsAsync(groupId, null, status, _currentOffset, _pageSize);
                    if (pagedList?.Items != null)
                    {
                        allSections.AddRange(pagedList.Items);
                        _currentOffset = pagedList.NextPageOffset ?? 0;
                    }
                    else
                    {
                        break;
                    }
                } while (pagedList?.NextPageOffset != null);

                _sections = allSections;
            }
            catch (Exception)
            {
                ToastService.ShowError("Problem prilikom dobavljanja clanova/sekcija!");
            }
        }

        public async Task LoadGroupSectionsAsync()
        {
            try
            {
                _group = await GroupService.GetGroupByIdAsync(groupId);
                if (_group == null)
                {
                    ToastService.ShowError("Problem prilikom dobavljanja podataka za grupe clanova!");
                }
            }
            catch (Exception)
            {
                ToastService.ShowError("Problem prilikom dobavljanja podataka za grupe clanova!");
            }
        }

        private void ToggleVisibility(ref bool visibilityFlag)
        {
            visibilityFlag = !visibilityFlag;
        }

        private void ToggleStatusFilter()
        {
            _isStatusFilterVisible = !_isStatusFilterVisible;
        }

        private async Task OnStatusFilterChanged(ChangeEventArgs e, SectionStatusType status)
        {
            var isChecked = (e.Value as bool?) ?? false;
            if (isChecked)
            {
                _selectedStatuses.Add(status);
            }
            else
            {
                _selectedStatuses.Remove(status);
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
            _sectionModalMode = ModalMode.EDIT;
            _isSectionModalVisible = true;
        }

        /// <summary>
        /// Metoda koja je kreirana da otvori modal u VIEW modu i da proslijedi odabranu sekciju
        /// </summary>
        private void OpenHistoricalSectionModal()
        {
            _sectionModalMode = ModalMode.VIEW;
            _isSectionModalVisible = true;
        }

        /// <summary>
        /// Metoda koja sluzi da otvori SectionModal
        /// </summary>
        private void ShowSectionModalForInsert()
        {
            _selectedSection = null;
            _isSectionModalVisible = true;
            _sectionModalMode = ModalMode.INSERT;
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
            _selectedSection = section;
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