using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;

namespace AutoDocFront.Components.Pages;

/// <summary>
/// Stranica za prikaz grešaka sa osnovnim informacijama o zahtjevu.
/// </summary>
public partial class Error
{
    [CascadingParameter] private HttpContext? HttpContext { get; set; }

    private string? RequestId { get; set; }
    private bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    /// <summary>
    /// Postavlja identifikator zahtjeva prilikom inicijalizacije.
    /// </summary>
    protected override void OnInitialized()
    {
        RequestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier;
    }
}
