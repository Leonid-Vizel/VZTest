using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VZTest.Migrations
{
    public partial class ArchitectUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Options_OptionId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Options_RadioAnswerOptional_OptionId",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_OptionId",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_RadioAnswerOptional_OptionId",
                table: "Answers");

            migrationBuilder.AddColumn<string>(
                name: "CheckAnswerOptional_InternalData",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternalData",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CorrectAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorrectAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CorrectAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CorrectAnswers_QuestionId",
                table: "CorrectAnswers",
                column: "QuestionId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CorrectAnswers");

            migrationBuilder.DropColumn(
                name: "CheckAnswerOptional_InternalData",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "InternalData",
                table: "Answers");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_OptionId",
                table: "Answers",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_RadioAnswerOptional_OptionId",
                table: "Answers",
                column: "RadioAnswerOptional_OptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Options_OptionId",
                table: "Answers",
                column: "OptionId",
                principalTable: "Options",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Options_RadioAnswerOptional_OptionId",
                table: "Answers",
                column: "RadioAnswerOptional_OptionId",
                principalTable: "Options",
                principalColumn: "Id");
        }
    }
}
