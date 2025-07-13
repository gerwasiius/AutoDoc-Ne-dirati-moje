using System;

namespace AutoDocFront.Utilities;

public static class PlaceholderHelpers
{
    public static string GetTypeBadgeClass(string type) => type switch
    {
        "STRING" => "bg-primary",
        "INT" => "bg-info text-dark",
        "DECIMAL" => "bg-success",
        "DATETIME" => "bg-secondary",
        "ENUM" => "bg-warning text-dark",
        "CHAR" => "bg-danger",
        _ => "bg-light text-dark"
    };
}
