namespace AutoDocService.DL.DTO.TemplateSectionsRelationDTO
{
    public class TemplateSectionRelationFlatDTO
    {
        public int RelationId { get; set; }
        public int SectionId { get; set; }
        public int SectionVersion { get; set; }
        public int Order { get; set; }
        public string ConditionExpression { get; set; }
        public string ActionType { get; set; }
        public bool IsPageBreak { get; set; }
        public bool IsArticle { get; set; }

        // Section info
        public int SectionDbId { get; set; }
        public string SectionName { get; set; }
        public string SectionDescription { get; set; }
    }
}
