namespace AutoDocService.Helpers.Utils
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PlaceholderAttribute : Attribute
    {
        public string Label { get; }
        public string Description { get; }
        //public string DataType { get; }

        public PlaceholderAttribute(string label, string description)
        {
            Label = label;
            Description = description;
            //DataType = dataType;
        }
    }
}
