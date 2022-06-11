using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VZTest.Migrations
{
    public partial class AddedCorrectToEF : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Balls",
                table: "Attempts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "CorrectAnswers",
                table: "Attempts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Correct",
                table: "Answers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balls",
                table: "Attempts");

            migrationBuilder.DropColumn(
                name: "CorrectAnswers",
                table: "Attempts");

            migrationBuilder.DropColumn(
                name: "Correct",
                table: "Answers");
        }
    }
}
