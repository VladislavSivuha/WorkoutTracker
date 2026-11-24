using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WorkoutTracker.Migrations
{
    /// <inheritdoc />
    public partial class InitTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagUrl",
                table: "Exercise",
                newName: "ImageUrl");

            migrationBuilder.CreateTable(
                name: "TemplateHeader",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateHeader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateHeader_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemplateSpecification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    TemplateHeaderId = table.Column<int>(type: "integer", nullable: false),
                    ExerciseId = table.Column<int>(type: "integer", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateSpecification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateSpecification_Exercise_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemplateSpecification_TemplateHeader_TemplateHeaderId",
                        column: x => x.TemplateHeaderId,
                        principalTable: "TemplateHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TemplateHeader_UserId",
                table: "TemplateHeader",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateSpecification_ExerciseId",
                table: "TemplateSpecification",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateSpecification_TemplateHeaderId",
                table: "TemplateSpecification",
                column: "TemplateHeaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemplateSpecification");

            migrationBuilder.DropTable(
                name: "TemplateHeader");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Exercise",
                newName: "ImagUrl");
        }
    }
}
