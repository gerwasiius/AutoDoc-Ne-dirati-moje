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

        private string GetTypeClass(string type) => type switch
        {
            "string" => "bg-primary",
            "int" => "bg-info text-dark",
            "decimal" => "bg-success",
            "DateTime" => "bg-secondary",
            "enum" => "bg-warning text-dark",
            "char" => "bg-danger",
            _ => "bg-light text-dark"
        };
    }
}
