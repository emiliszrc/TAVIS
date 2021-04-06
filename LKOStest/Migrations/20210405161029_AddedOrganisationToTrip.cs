using Microsoft.EntityFrameworkCore.Migrations;

namespace LKOStest.Migrations
{
    public partial class AddedOrganisationToTrip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrganisationId",
                table: "Trips",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trips_OrganisationId",
                table: "Trips",
                column: "OrganisationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Organisations_OrganisationId",
                table: "Trips",
                column: "OrganisationId",
                principalTable: "Organisations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Organisations_OrganisationId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_OrganisationId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "Trips");
        }
    }
}
