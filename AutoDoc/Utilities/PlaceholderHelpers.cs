using System;

namespace AutoDocFront.Utilities;

public static class PlaceholderHelpers
{
    public static string GetTypeBadgeClass(string type) => type switch
    {
        "string" => "bg-primary",
        "int" => "bg-info text-dark",
        "decimal" => "bg-success",
        "DateTime" => "bg-secondary",
        "enum" => "bg-warning text-dark",
        "char" => "bg-danger",
        _ => "bg-light text-dark"
    };
}
