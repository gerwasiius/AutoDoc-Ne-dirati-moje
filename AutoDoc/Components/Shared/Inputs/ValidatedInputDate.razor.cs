using Microsoft.AspNetCore.Components;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace AutoDocFront.Components.Shared.Inputs
{
    public partial class ValidatedInputDate<TValue>
    {
        [Parameter] public string Label { get; set; }
        [Parameter] public bool IsRequired { get; set; } = false;
        [Parameter] public bool Disabled { get; set; } = false;
        [Parameter] public TValue Value { get; set; }
        [Parameter] public EventCallback<TValue> ValueChanged { get; set; }
        [Parameter] public Expression<Func<TValue>> FieldExpression { get; set; }
        [Parameter] public string InputClass { get; set; }
        [Parameter] public string InputStyle { get; set; }

        private string CssClass => $"form-control{(string.IsNullOrWhiteSpace(InputClass) ? "" : $" {InputClass}")}";


        private async Task OnValueChanged(ChangeEventArgs e)
        {
            if (ValueChanged.HasDelegate)
            {
                await ValueChanged.InvokeAsync((TValue)e.Value);
            }
        }
    }
}
