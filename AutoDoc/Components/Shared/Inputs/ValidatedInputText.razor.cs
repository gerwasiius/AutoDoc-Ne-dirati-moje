using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace AutoDocFront.Components.Shared.Inputs
{
    public partial class ValidatedInputText
    {
        [Parameter] public string Label { get; set; }
        [Parameter] public string Placeholder { get; set; }
        [Parameter] public bool IsRequired { get; set; } = false;
        [Parameter] public bool Disabled { get; set; } = false;
        [Parameter] public string FieldId { get; set; }
        [Parameter] public string Value { get; set; }
        [Parameter] public EventCallback<string> ValueChanged { get; set; }
        [Parameter] public Expression<Func<string>> FieldExpression { get; set; }
        [Parameter] public bool EnableValidation { get; set; } = true;
        [Parameter] public string InputClass { get; set; } = "";
        [Parameter] public string InputStyle { get; set; } = "";

        private string CssClass => $"form-control{(string.IsNullOrWhiteSpace(InputClass) ? "" : $" {InputClass}")}";

        private async Task OnValueChanged(string value)
        {
            Value = value; // update local value
            await ValueChanged.InvokeAsync(value);
        }
    }
}
