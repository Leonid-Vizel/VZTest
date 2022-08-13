using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VZTest.Migrations
{
    public partial class CorrectAnswerModelUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckAnswerString",
                table: "CorrectAnswers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "CorrectAnswers");

            migrationBuilder.DropColumn(
                name: "CheckAnswerString",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "RadioAnswer",
                table: "Answers");

            migrationBuilder.RenameColumn(
                name: "CorrectTextAnswer_Correct",
                table: "CorrectAnswers",
                newName: "TextAnswer");

            migrationBuilder.RenameColumn(
                name: "CorrectIntAnswer_Correct",
                table: "CorrectAnswers",
                newName: "IntAnswer");

            migrationBuilder.RenameColumn(
                name: "CorrectDoubleAnswer_Correct",
                table: "CorrectAnswers",
                newName: "DoubleAnswer");

            migrationBuilder.RenameColumn(
                name: "Correct",
                table: "CorrectAnswers",
                newName: "DateAnswer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TextAnswer",
                table: "CorrectAnswers",
                newName: "CorrectTextAnswer_Correct");

            migrationBuilder.RenameColumn(
                name: "IntAnswer",
                table: "CorrectAnswers",
                newName: "CorrectIntAnswer_Correct");

            migrationBuilder.RenameColumn(
                name: "DoubleAnswer",
                table: "CorrectAnswers",
                newName: "CorrectDoubleAnswer_Correct");

            migrationBuilder.RenameColumn(
                name: "DateAnswer",
                table: "CorrectAnswers",
                newName: "Correct");

            migrationBuilder.AddColumn<string>(
                name: "CheckAnswerString",
                table: "CorrectAnswers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "CorrectAnswers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CheckAnswerString",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RadioAnswer",
                table: "Answers",
                type: "int",
                nullable: true);
        }
    }
}
