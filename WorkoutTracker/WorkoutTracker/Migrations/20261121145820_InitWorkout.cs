using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WorkoutTracker.Migrations
{
    /// <inheritdoc />
    public partial class InitWorkout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkoutHeader",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FinishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutHeader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutHeader_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutSpecification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    WorkoutHeaderId = table.Column<int>(type: "integer", nullable: false),
                    ExerciseId = table.Column<int>(type: "integer", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutSpecification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutSpecification_Exercise_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkoutSpecification_WorkoutHeader_WorkoutHeaderId",
                        column: x => x.WorkoutHeaderId,
                        principalTable: "WorkoutHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    WorkoutSpecificationId = table.Column<int>(type: "integer", nullable: false),
                    SetNumber = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<decimal>(type: "numeric", nullable: false),
                    Reps = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutSet_WorkoutSpecification_WorkoutSpecificationId",
                        column: x => x.WorkoutSpecificationId,
                        principalTable: "WorkoutSpecification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutHeader_UserId",
                table: "WorkoutHeader",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSet_WorkoutSpecificationId",
                table: "WorkoutSet",
                column: "WorkoutSpecificationId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSpecification_ExerciseId",
                table: "WorkoutSpecification",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSpecification_WorkoutHeaderId",
                table: "WorkoutSpecification",
                column: "WorkoutHeaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkoutSet");

            migrationBuilder.DropTable(
                name: "WorkoutSpecification");

            migrationBuilder.DropTable(
                name: "WorkoutHeader");
        }
    }
}
