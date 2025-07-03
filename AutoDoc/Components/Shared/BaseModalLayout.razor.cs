using Microsoft.AspNetCore.Components;

namespace AutoDocFront.Components.Shared;

public partial class BaseModalLayout
{
    [Parameter] public bool IsOpen { get; set; }
    [Parameter] public string Title { get; set; } = string.Empty;
    [Parameter] public string ModalSize { get; set; } = "modal-lg";
    [Parameter] public EventCallback<bool> IsOpenChanged { get; set; }
    [Parameter] public RenderFragment? BodyContent { get; set; }
    [Parameter] public RenderFragment? FooterContent { get; set; }

    private async Task CloseModal()
    {
        await IsOpenChanged.InvokeAsync(false);
    }
}