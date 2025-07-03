using Microsoft.AspNetCore.Components;

namespace AutoDocFront.Components.Shared;

/// <summary>
/// Apstraktna baza za modalne komponente sa osnovnom logikom za zatvaranje.
/// </summary>
public abstract class ModalBase : ComponentBase
{
    /// <summary>
    /// Označava da li je modal otvoren.
    /// </summary>
    [Parameter] public bool IsOpen { get; set; }

    /// <summary>
    /// Event za promjenu stanja modala.
    /// </summary>
    [Parameter] public EventCallback<bool> IsOpenChanged { get; set; }

    /// <summary>
    /// Zatvara modal i emituje promjenu stanja.
    /// </summary>
    protected async Task CloseAsync()
    {
        IsOpen = false;
        await IsOpenChanged.InvokeAsync(false);
    }
}
