using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace AutoDocFront.Components.Shared.Inputs
{
    public partial class ValidatedInputDate<TValue>
    {
        [Parameter] public string Label { get; set; }
        [Parameter] public string Placeholder { get; set; }
        [Parameter] public bool IsRequired { get; set; } = false;
        [Parameter] public bool Disabled { get; set; } = false;
        [Parameter] public string FieldId { get; set; }
        [Parameter] public string InputClass { get; set; } = "";
        [Parameter] public string InputStyle { get; set; } = "";

        [Parameter] public TValue Value { get; set; }
        [Parameter] public EventCallback<TValue> ValueChanged { get; set; }
        [Parameter] public Expression<Func<TValue>> FieldExpression { get; set; }

        private string CssClass => $"form-control{(string.IsNullOrWhiteSpace(InputClass) ? "" : $" {InputClass}")}";
        private async Task OnValueChanged(TValue newValue)
        {
            Value = newValue;
            await ValueChanged.InvokeAsync(newValue);
        }
    }
}