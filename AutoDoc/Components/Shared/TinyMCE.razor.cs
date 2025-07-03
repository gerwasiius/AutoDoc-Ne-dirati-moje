using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AutoDocFront.Components.Shared;

public partial class TinyMCE : IDisposable
{
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    [Parameter] public string Id { get; set; } = $"tinymce-{Guid.NewGuid()}";
    [Parameter] public string Value { get; set; } = string.Empty;
    [Parameter] public EventCallback<string> ValueChanged { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("initializeTinyMCE", $"#{Id}", Value);
        }
    }

    public async Task UpdateContentFromEditor()
    {
        Value = await JSRuntime.InvokeAsync<string>("getEditorContent", Id);
        await ValueChanged.InvokeAsync(Value);
    }

    public async Task DestroyEditor()
    {
        await JSRuntime.InvokeVoidAsync("destroyTinyMCE", Id);
    }

    public void Dispose()
    {
        _ = DestroyEditor();
    }
}
