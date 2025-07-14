using AutoDocFront.Models;
using System.Text.RegularExpressions;

namespace AutoDocFront.Utilities
{
    public static class ConditionSerializer
    {
        public static string Serialize(ConditionNode node)
        {
            if (node is ConditionLeaf leaf)
                return $"{leaf.Left} {leaf.Operator} \"{leaf.Right}\"";
            if (node is ConditionGroup group)
            {
                var joined = string.Join($" {group.LogicalOperator} ", group.Children.Select(Serialize));
                return $"({joined})";
            }
            return "";
        }

        public static ConditionLeaf ParseLeaf(string expr)
        {
            // Example input: Client.Ime == "Zlatan"
            // Regex: (left) (operator) (right)
            var match = Regex.Match(expr, @"^(?<left>[\w\.]+)\s*(?<op>==|!=|>|<|>=|<=)\s*(?<right>.+)$");
            if (!match.Success)
                throw new ArgumentException($"Neispravan format izraza: {expr}");

            var left = match.Groups["left"].Value.Trim();
            var op = match.Groups["op"].Value.Trim();
            var right = match.Groups["right"].Value.Trim();

            // Remove quotes if present
            if (right.StartsWith("\"") && right.EndsWith("\""))
                right = right.Substring(1, right.Length - 2);

            return new ConditionLeaf
            {
                Left = left,
                Operator = op,
                Right = right
            };
        }
    }
}
