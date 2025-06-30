using AutoDocService.DL.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoDocService.DL.DBContext
{
    /// <summary>
    /// DBContext of database
    /// </summary>
    public class ContractGenerationContext : DbContext
    {
        #region DbSet
        /// <summary>
        /// SectionGroup DbSet
        /// </summary>
        public virtual DbSet<SectionGroup> SectionGroups { get; set; }
        /// <summary>
        /// DocumentTemplates DbSet
        /// </summary>
        public virtual DbSet<DocumentTemplates> DocumentTemplates { get; set; }
        /// <summary>
        /// Sections DbSet
        /// </summary>
        public virtual DbSet<Sections> Sections { get; set; }
        /// <summary>
        /// TemplateSectionsRelations DbSet
        /// </summary>
        public virtual DbSet<TemplateSectionsRelation> TemplateSectionsRelations { get; set; }
        #endregion

        /// <summary>
        /// Context constructor
        /// </summary>
        /// <param name="options"></param>
        public ContractGenerationContext(DbContextOptions<ContractGenerationContext> options)
           : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region SectionGroup
            modelBuilder.Entity<SectionGroup>(entity =>
            {
                entity.ToTable("SectionGroup", "dbo");
                entity.HasKey(e => e.ID);
                entity.Property(e => e.Name).HasColumnName("Name").HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.Description).HasColumnName("Description").HasMaxLength(250).IsUnicode(false);
                entity.Property(e => e.Status).HasColumnName("Status").HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.DateInserted).HasColumnName("DateInserted").HasColumnType("datetime");
                entity.Property(e => e.UserInserted).HasColumnName("UserInserted").HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.DateUpdated).HasColumnName("DateUpdated").HasColumnType("datetime");
                entity.Property(e => e.UserUpdated).HasColumnName("UserUpdated").HasMaxLength(50).IsUnicode(false);
            });
            #endregion
            #region DocumentTemplate
            modelBuilder.Entity<DocumentTemplates>(entity =>
            {
                entity.ToTable("DocumentTemplate", "dbo");

                entity.HasIndex(e => new { e.IdTemplate, e.Version }, "UC_DocumentTemplate").IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.DateInserted).HasDefaultValueSql("GETDATE()").HasColumnType("datetime");
                entity.Property(e => e.DateUpdated).HasColumnType("datetime");
                entity.Property(e => e.DateVerified).HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(250).IsUnicode(false);
                entity.Property(e => e.IdTemplate);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.UserInserted).IsRequired().HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.UserUpdated).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.UserVerified).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.ValidFrom).HasColumnType("datetime");
                entity.Property(e => e.ValidTo).HasColumnType("datetime");
                entity.Property(e => e.Version);
            });

            #endregion
            #region Sections
            modelBuilder.Entity<Sections>(entity =>
            {
                entity.ToTable("Sections", "dbo");

                entity.HasIndex(e => new { e.IdSection, e.Version }, "UC_Sections").IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Content);
                entity.Property(e => e.DateInserted).HasDefaultValueSql("GETDATE()").HasColumnType("datetime");
                entity.Property(e => e.DateUpdated).HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(250).IsUnicode(false);
                entity.Property(e => e.GroupId);
                entity.Property(e => e.IdSection);
                entity.Property(e => e.IsActive);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.UserInserted).IsRequired().HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.UserUpdated).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Version);
            });
            #endregion

            #region TemplateSectionsRelation
            modelBuilder.Entity<TemplateSectionsRelation>(entity =>
            {
                entity.ToTable("TemplateSectionsRelation", "dbo");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.IdSection);
                entity.Property(e => e.IdTemplate);
                entity.Property(e => e.SectionVersion);
                entity.Property(e => e.TemplateVersion);
                entity.Property(e => e.Order).HasColumnName("Order");
                entity.Property(e => e.ConditionExpression).HasMaxLength(300);
                entity.Property(e => e.ActionType).IsRequired().HasMaxLength(20);
                entity.Property(e => e.IsPageBreak).HasDefaultValue(0);
                entity.Property(e => e.IsArticle).HasDefaultValue(0);


                entity.HasOne(d => d.Section).WithMany(p => p.TemplateSectionsRelations)
                    .HasPrincipalKey(p => new { p.IdSection, p.Version })
                    .HasForeignKey(d => new { d.IdSection, d.SectionVersion })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TemplateSections__7DD962F1");

                entity.HasOne(d => d.DocumentTemplate).WithMany(p => p.TemplateSectionsRelations)
                    .HasPrincipalKey(p => new { p.IdTemplate, p.Version })
                    .HasForeignKey(d => new { d.IdTemplate, d.TemplateVersion })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TemplateSections__7CE53EB8");
            });
            #endregion
        }
    }
}

