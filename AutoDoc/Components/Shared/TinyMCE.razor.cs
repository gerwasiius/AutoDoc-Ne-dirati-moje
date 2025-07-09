using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AutoDocFront.Components.Shared;

public partial class TinyMCE : IDisposable
{
    [Parameter] public string Id { get; set; } = $"tinymce-{Guid.NewGuid()}";
    [Parameter] public string Value { get; set; } = string.Empty;
    [Parameter] public EventCallback<string> ValueChanged { get; set; }

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
    [Inject] private IToastService ToastService { get; set; } = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                await JSRuntime.InvokeVoidAsync("initializeTinyMCE", $"#{Id}", Value);
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška prilikom inicijalizacije TinyMCE editora: {ex.Message}");
            }
        }
    }

    public async Task UpdateContentFromEditor()
    {
        try
        {
            Value = await JSRuntime.InvokeAsync<string>("getEditorContent", Id);
            await ValueChanged.InvokeAsync(Value);
        }
        catch (Exception ex)
        {
            ToastService.ShowError($"Greška prilikom preuzimanja sadržaja iz TinyMCE editora: {ex.Message}");
        }
    }


    public async Task DestroyEditor()
    {
        try
        {
            await JSRuntime.InvokeVoidAsync("destroyTinyMCE", Id);
        }
        catch (Exception ex)
        {
            ToastService.ShowError($"Greška prilikom uništavanja TinyMCE editora: {ex.Message}");
        }
    }

    public void Dispose()
    {
        // Fire and forget, but catch errors
        _ = SafeDestroyEditor();
    }

    private async Task SafeDestroyEditor()
    {
        try
        {
            await DestroyEditor();
        }
        catch
        {
            // swallow, already logged in DestroyEditor
        }
    }
}