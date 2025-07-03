using AutoDocFront.Services;
using AutoDocFront.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;

namespace AutoDocFront.Components.Modals
{
    public partial class TemplatePreviewModal : ModalBase
    {
        // Parametri premješteni u baznu klasu
        [Parameter] public string TemplateName { get; set; }
        [Parameter] public string HtmlContent { get; set; }
        [Parameter] public bool IsLoading { get; set; }
        [Parameter] public string ErrorMessage { get; set; }
    }
}
