using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoDocService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "DocumentTemplate",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdTemplate = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", unicode: false, maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", unicode: false, maxLength: 250, nullable: true),
                    Version = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", unicode: false, maxLength: 20, nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime", nullable: true),
                    ValidTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateInsert = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    UserInsert = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false),
                    DateUpdate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserUpdate = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: true),
                    DateVerified = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserVerified = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTemplate", x => x.ID);
                    table.UniqueConstraint("AK_DocumentTemplate_IdTemplate_Version", x => new { x.IdTemplate, x.Version });
                });

            migrationBuilder.CreateTable(
                name: "SectionGroup",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", unicode: false, maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "TEXT", unicode: false, maxLength: 250, nullable: true),
                    Status = table.Column<string>(type: "TEXT", unicode: false, maxLength: 20, nullable: true),
                    DateInserted = table.Column<DateTime>(type: "datetime", nullable: false),
                    UserInserted = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserUpdated = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionGroup", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdSection = table.Column<int>(type: "INTEGER", nullable: false),
                    GroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", unicode: false, maxLength: 250, nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    Version = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateInsert = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "GETDATE()"),
                    UserInsert = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false),
                    DateUpdate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserUpdate = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.ID);
                    table.UniqueConstraint("AK_Sections_IdSection_Version", x => new { x.IdSection, x.Version });
                });

            migrationBuilder.CreateTable(
                name: "TemplateSectionsRelation",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdTemplate = table.Column<int>(type: "INTEGER", nullable: false),
                    TemplateVersion = table.Column<int>(type: "INTEGER", nullable: false),
                    IdSection = table.Column<int>(type: "INTEGER", nullable: false),
                    SectionVersion = table.Column<int>(type: "INTEGER", nullable: false),
                    SectionOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Condition = table.Column<string>(type: "TEXT", nullable: true),
                    Action = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false),
                    PageBreak = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateSectionsRelation", x => x.ID);
                    table.ForeignKey(
                        name: "FK__TemplateSections__7CE53EB8",
                        columns: x => new { x.IdTemplate, x.TemplateVersion },
                        principalSchema: "dbo",
                        principalTable: "DocumentTemplate",
                        principalColumns: new[] { "IdTemplate", "Version" });
                    table.ForeignKey(
                        name: "FK__TemplateSections__7DD962F1",
                        columns: x => new { x.IdSection, x.SectionVersion },
                        principalSchema: "dbo",
                        principalTable: "Sections",
                        principalColumns: new[] { "IdSection", "Version" });
                });

            migrationBuilder.CreateIndex(
                name: "UC_DocumentTemplate",
                schema: "dbo",
                table: "DocumentTemplate",
                columns: new[] { "IdTemplate", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UC_Sections",
                schema: "dbo",
                table: "Sections",
                columns: new[] { "IdSection", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TemplateSectionsRelation_IdSection_SectionVersion",
                schema: "dbo",
                table: "TemplateSectionsRelation",
                columns: new[] { "IdSection", "SectionVersion" });

            migrationBuilder.CreateIndex(
                name: "IX_TemplateSectionsRelation_IdTemplate_TemplateVersion",
                schema: "dbo",
                table: "TemplateSectionsRelation",
                columns: new[] { "IdTemplate", "TemplateVersion" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SectionGroup",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TemplateSectionsRelation",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "DocumentTemplate",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Sections",
                schema: "dbo");
        }
    }
}
