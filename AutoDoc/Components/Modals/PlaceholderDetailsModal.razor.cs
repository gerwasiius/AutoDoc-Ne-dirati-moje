using AutoDoc.Shared.Model.Placeholders.PlaceholderMetadata;
using AutoDocFront.Components.Shared;
using Microsoft.AspNetCore.Components;

namespace AutoDocFront.Components.Modals
{
    public partial class PlaceholderDetailsModal : ModalBase
    {
        [Parameter] public PlaceholderMeta? Placeholder { get; set; }

        private async Task Close()
        {
            IsOpen = false;
            await IsOpenChanged.InvokeAsync(false);
        }
    }
}
