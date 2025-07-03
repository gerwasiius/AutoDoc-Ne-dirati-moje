using System.Reflection;
using System.ComponentModel.DataAnnotations;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;

namespace AutoDocFront.Components.Shared;

public partial class PlaceholderComponent
{
    [Parameter] public List<PropertyInfo> ParameterProperties { get; set; } = new();

    [Inject] private IJSRuntime JS { get; set; } = default!;

    private void ToggleSubProperties(string propertyName) =>
        JS.InvokeVoidAsync("toggleCollapse", $"#{propertyName}");

    private static bool IsComplexProperty(PropertyInfo property) =>
        property.PropertyType.IsClass && property.PropertyType != typeof(string);

    private static IEnumerable<PropertyInfo> GetSubProperties(PropertyInfo property) =>
        property.PropertyType.GetProperties().Where(p => p.CanRead && p.GetMethod!.IsPublic);

    private static string GetDisplayName(PropertyInfo property)
    {
        var displayAttribute = property.GetCustomAttribute<DisplayAttribute>();
        return displayAttribute != null ? displayAttribute.Name : property.Name;
    }

    private async Task InsertPlaceholder(string placeholder)
    {
        await JS.InvokeVoidAsync("insertPlaceholder", placeholder);
    }
}
