using Microsoft.EntityFrameworkCore.Migrations;

namespace LKOStest.Migrations
{
    public partial class AddTripWarningVisits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VisitId",
                table: "Warnings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Warnings_VisitId",
                table: "Warnings",
                column: "VisitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Warnings_Visits_VisitId",
                table: "Warnings",
                column: "VisitId",
                principalTable: "Visits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Warnings_Visits_VisitId",
                table: "Warnings");

            migrationBuilder.DropIndex(
                name: "IX_Warnings_VisitId",
                table: "Warnings");

            migrationBuilder.DropColumn(
                name: "VisitId",
                table: "Warnings");
        }
    }
}
