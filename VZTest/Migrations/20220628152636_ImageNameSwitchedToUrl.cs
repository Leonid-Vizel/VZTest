using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VZTest.Migrations
{
    public partial class ImageNameSwitchedToUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CorrectAnswers_Questions_QuestionId",
                table: "CorrectAnswers");

            migrationBuilder.DropIndex(
                name: "IX_CorrectAnswers_QuestionId",
                table: "CorrectAnswers");

            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "Questions",
                newName: "ImageUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Questions",
                newName: "ImageName");

            migrationBuilder.CreateIndex(
                name: "IX_CorrectAnswers_QuestionId",
                table: "CorrectAnswers",
                column: "QuestionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CorrectAnswers_Questions_QuestionId",
                table: "CorrectAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
