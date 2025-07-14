namespace AutoDocFront.Models
{
    public abstract class ConditionNode { }

    public class ConditionGroup : ConditionNode
    {
        public List<ConditionNode> Children { get; set; } = new();
        public string LogicalOperator { get; set; } = "&&";
    }

    public class ConditionLeaf : ConditionNode
    {
        public string Left { get; set; }
        public string Operator { get; set; }
        public string Right { get; set; }
    }
}
