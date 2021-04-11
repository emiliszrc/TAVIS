using Microsoft.EntityFrameworkCore.Migrations;

namespace LKOStest.Migrations
{
    public partial class AddedNotifiedStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TripId",
                table: "SentEmails",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Notified",
                table: "Clients",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_SentEmails_TripId",
                table: "SentEmails",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_SentEmails_Trips_TripId",
                table: "SentEmails",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SentEmails_Trips_TripId",
                table: "SentEmails");

            migrationBuilder.DropIndex(
                name: "IX_SentEmails_TripId",
                table: "SentEmails");

            migrationBuilder.DropColumn(
                name: "TripId",
                table: "SentEmails");

            migrationBuilder.DropColumn(
                name: "Notified",
                table: "Clients");
        }
    }
}
