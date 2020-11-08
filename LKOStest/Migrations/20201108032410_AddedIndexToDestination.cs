using Microsoft.EntityFrameworkCore.Migrations;

namespace LKOStest.Migrations
{
    public partial class AddedIndexToDestination : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Index",
                table: "Destinations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Index",
                table: "Destinations");
        }
    }
}
