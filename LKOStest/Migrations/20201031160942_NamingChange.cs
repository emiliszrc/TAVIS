using Microsoft.EntityFrameworkCore.Migrations;

namespace LKOStest.Migrations
{
    public partial class NamingChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Trips");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Trips",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Trips");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
