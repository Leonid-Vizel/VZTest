using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VZTest.Migrations
{
    public partial class FixingTheAnswerSystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Correct",
                table: "CorrectAnswers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CorrectDoubleAnswer_Correct",
                table: "CorrectAnswers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CorrectIntAnswer_Correct",
                table: "CorrectAnswers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorrectTextAnswer_Correct",
                table: "CorrectAnswers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Correct",
                table: "CorrectAnswers");

            migrationBuilder.DropColumn(
                name: "CorrectDoubleAnswer_Correct",
                table: "CorrectAnswers");

            migrationBuilder.DropColumn(
                name: "CorrectIntAnswer_Correct",
                table: "CorrectAnswers");

            migrationBuilder.DropColumn(
                name: "CorrectTextAnswer_Correct",
                table: "CorrectAnswers");
        }
    }
}
