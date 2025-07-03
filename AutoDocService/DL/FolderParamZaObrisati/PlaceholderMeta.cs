namespace AutoDocService.DL.FolderParamZaObrisati
{
    public class PlaceholderMeta
    {
        public string Id { get; set; }           // npr. "Grupa1.Placeholder1"
        public string Group { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public bool IsNullable { get; set; }
        public List<string> EnumValues { get; set; }
    }
}
