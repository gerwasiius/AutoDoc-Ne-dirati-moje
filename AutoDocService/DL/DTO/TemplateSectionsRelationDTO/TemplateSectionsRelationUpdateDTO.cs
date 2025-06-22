namespace AutoDocService.DL.DTO.TemplateSectionsRelationDTO
{
    /// <summary>
    /// Update DTO for TemplateSectionsRelation
    /// </summary>
    public class TemplateSectionsRelationUpdateDTO : TemplateSectionsRelationBaseDTO
    {
        /// <summary>
        /// Jedinstveni identifikator za vezu između predloška i sekcije
        /// </summary>
        public int Id { get; set; }
    }
}
