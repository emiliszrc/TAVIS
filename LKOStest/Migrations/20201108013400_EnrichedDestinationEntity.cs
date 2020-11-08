using Microsoft.EntityFrameworkCore.Migrations;

namespace LKOStest.Migrations
{
    public partial class EnrichedDestinationEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Destinations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                table: "Destinations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Longtitude",
                table: "Destinations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Destinations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "Longtitude",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Destinations");
        }
    }
}
