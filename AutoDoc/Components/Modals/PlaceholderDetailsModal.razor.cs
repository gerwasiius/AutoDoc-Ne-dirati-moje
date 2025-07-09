using AutoDoc.Shared.Model.Placeholders;
using AutoDocFront.Components.Shared;
using AutoDocFront.Utilities;
using Microsoft.AspNetCore.Components;

namespace AutoDocFront.Components.Modals
{
    public partial class PlaceholderDetailsModal : ModalBase
    {
        [Parameter] public PlaceholderMetadata? Placeholder { get; set; }

        private async Task Close()
        {
            IsOpen = false;
            await IsOpenChanged.InvokeAsync(false);
        }
    }
}
