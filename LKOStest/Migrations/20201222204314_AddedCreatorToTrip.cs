using Microsoft.EntityFrameworkCore.Migrations;

namespace LKOStest.Migrations
{
    public partial class AddedCreatorToTrip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Trips",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trips_CreatorId",
                table: "Trips",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Users_CreatorId",
                table: "Trips",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Users_CreatorId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_CreatorId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Trips");
        }
    }
}
