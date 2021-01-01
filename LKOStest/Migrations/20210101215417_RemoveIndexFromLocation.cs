using Microsoft.EntityFrameworkCore.Migrations;

namespace LKOStest.Migrations
{
    public partial class RemoveIndexFromLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Locations_LocationId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_LocationId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Index",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "VisitId",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_VisitId",
                table: "Comments",
                column: "VisitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Visits_VisitId",
                table: "Comments",
                column: "VisitId",
                principalTable: "Visits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Visits_VisitId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_VisitId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "VisitId",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "Index",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_LocationId",
                table: "Comments",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Locations_LocationId",
                table: "Comments",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
