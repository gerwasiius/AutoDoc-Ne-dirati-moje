using AutoDocFront.Models.DTO.GroupSection;
using AutoDocFront.Models.DTO.Sections;
using Microsoft.AspNetCore.Components;

namespace AutoDocFront.Components.Modals
{
    public partial class SectionPickerDialog
    {
        [Parameter] public bool IsOpen { get; set; }
        [Parameter] public EventCallback<bool> IsOpenChanged { get; set; }
        [Parameter] public List<SectionGroupGetDTO> AvailableGroups { get; set; } = new();
        [Parameter] public List<SectionsGetDTO> AvailableSections { get; set; } = new();
        [Parameter] public EventCallback<List<SectionsGetDTO>> OnSectionsPicked { get; set; }
        [Parameter] public EventCallback<string> OnGroupChanged { get; set; }


        private string SelectedGroup = "";
        private string SectionSearchTerm = "";
        private HashSet<int> SelectedSectionIds = new();

        //private IEnumerable<Section> FilteredSections =>
        //    AvailableSections
        //        .Where(s => (string.IsNullOrEmpty(SelectedGroup) || s.GroupName == SelectedGroup)
        //            && (string.IsNullOrEmpty(SectionSearchTerm) || s.Name.Contains(SectionSearchTerm, StringComparison.OrdinalIgnoreCase)));

        private void ToggleSectionSelection(int id, object? checkedValue)
        {
            if ((bool?)checkedValue == true)
                SelectedSectionIds.Add(id);
            else
                SelectedSectionIds.Remove(id);
        }

        private async Task AddSelectedSections()
        {
            var picked = AvailableSections.Where(s => SelectedSectionIds.Contains(s.ID)).ToList();
            await OnSectionsPicked.InvokeAsync(picked);
            SelectedSectionIds.Clear();
            await IsOpenChanged.InvokeAsync(false);
        }

        private async Task Close()
        {
            SelectedSectionIds.Clear();
            await IsOpenChanged.InvokeAsync(false);
        }

        private async Task OnGroupChangedHandler()
        {
            if (OnGroupChanged.HasDelegate)
                await OnGroupChanged.InvokeAsync(SelectedGroup);
        }
    }
}
