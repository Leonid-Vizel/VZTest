using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VZTest.Migrations
{
    public partial class addedAnswerBalls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balls",
                table: "Attempts");

            migrationBuilder.DropColumn(
                name: "Correct",
                table: "Answers");

            migrationBuilder.AddColumn<double>(
                name: "Balls",
                table: "Answers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balls",
                table: "Answers");

            migrationBuilder.AddColumn<double>(
                name: "Balls",
                table: "Attempts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "Correct",
                table: "Answers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
