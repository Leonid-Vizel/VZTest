using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VZTest.Migrations
{
    public partial class AddedMaxBalls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MaxBalls",
                table: "Attempts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxBalls",
                table: "Attempts");
        }
    }
}
