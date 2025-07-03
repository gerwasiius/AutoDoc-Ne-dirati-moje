using AutoDocFront.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;

namespace AutoDocFront.Components.Modals
{
    public partial class TemplatePreviewModal : ComponentBase
    {
        [Parameter] public bool IsOpen { get; set; }
        [Parameter] public EventCallback<bool> IsOpenChanged { get; set; }
        [Parameter] public string TemplateName { get; set; }
        [Parameter] public string HtmlContent { get; set; }
        [Parameter] public bool IsLoading { get; set; }
        [Parameter] public string ErrorMessage { get; set; }

        private async Task Close()
        {
            await IsOpenChanged.InvokeAsync(false);
        }
    }
}
