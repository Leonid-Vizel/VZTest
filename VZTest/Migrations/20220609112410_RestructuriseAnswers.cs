using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VZTest.Migrations
{
    public partial class RestructuriseAnswers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answer",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "CheckAnswerOptional_InternalData",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "DoubleAnswerOptional_Answer",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "IntAnswerOptional_Answer",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "IntAnswer_Answer",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "InternalData",
                table: "Answers");

            migrationBuilder.RenameColumn(
                name: "TextAnswer_Answer",
                table: "Answers",
                newName: "TextAnswer");

            migrationBuilder.RenameColumn(
                name: "TextAnswerOptional_Answer",
                table: "Answers",
                newName: "CheckAnswerString");

            migrationBuilder.RenameColumn(
                name: "RadioAnswerOptional_OptionId",
                table: "Answers",
                newName: "RadioAnswer");

            migrationBuilder.RenameColumn(
                name: "OptionId",
                table: "Answers",
                newName: "IntAnswer");

            migrationBuilder.RenameColumn(
                name: "DoubleAnswer_Answer",
                table: "Answers",
                newName: "DoubleAnswer");

            migrationBuilder.RenameColumn(
                name: "DateAnswerOptional_Answer",
                table: "Answers",
                newName: "DateAnswer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TextAnswer",
                table: "Answers",
                newName: "TextAnswer_Answer");

            migrationBuilder.RenameColumn(
                name: "RadioAnswer",
                table: "Answers",
                newName: "RadioAnswerOptional_OptionId");

            migrationBuilder.RenameColumn(
                name: "IntAnswer",
                table: "Answers",
                newName: "OptionId");

            migrationBuilder.RenameColumn(
                name: "DoubleAnswer",
                table: "Answers",
                newName: "DoubleAnswer_Answer");

            migrationBuilder.RenameColumn(
                name: "DateAnswer",
                table: "Answers",
                newName: "DateAnswerOptional_Answer");

            migrationBuilder.RenameColumn(
                name: "CheckAnswerString",
                table: "Answers",
                newName: "TextAnswerOptional_Answer");

            migrationBuilder.AddColumn<double>(
                name: "Answer",
                table: "Answers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckAnswerOptional_InternalData",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "DoubleAnswerOptional_Answer",
                table: "Answers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IntAnswerOptional_Answer",
                table: "Answers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IntAnswer_Answer",
                table: "Answers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternalData",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
