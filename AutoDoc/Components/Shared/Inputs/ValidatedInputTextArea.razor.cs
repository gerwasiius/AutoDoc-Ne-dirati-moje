using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace AutoDocFront.Components.Shared.Inputs
{
    public partial class ValidatedInputTextArea
    {
        [Parameter] public string Label { get; set; }
        [Parameter] public string Placeholder { get; set; }
        [Parameter] public int Rows { get; set; } = 3;
        [Parameter] public bool IsRequired { get; set; } = false;
        [Parameter] public bool Disabled { get; set; } = false;
        [Parameter] public string FieldId { get; set; }

        [Parameter] public string Value { get; set; }
        [Parameter] public EventCallback<string> ValueChanged { get; set; }
        [Parameter] public Expression<Func<string>> FieldExpression { get; set; }
    }
}
