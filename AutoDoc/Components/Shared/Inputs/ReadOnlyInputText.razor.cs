using Microsoft.AspNetCore.Components;

namespace AutoDocFront.Components.Shared.Inputs
{
    public partial class ReadOnlyInputText
    {
        [Parameter] public string Label { get; set; }
        [Parameter] public string Value { get; set; }
        [Parameter] public string InputClass { get; set; } = "";
        [Parameter] public string InputStyle { get; set; } = "";
        [Parameter] public bool IsTextArea { get; set; } = false;
        [Parameter] public int Rows { get; set; } = 1;
    }
}
