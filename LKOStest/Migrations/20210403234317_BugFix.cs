using Microsoft.EntityFrameworkCore.Migrations;

namespace LKOStest.Migrations
{
    public partial class BugFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Organisations");

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Organisations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organisations_CreatorId",
                table: "Organisations",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organisations_Users_CreatorId",
                table: "Organisations",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organisations_Users_CreatorId",
                table: "Organisations");

            migrationBuilder.DropIndex(
                name: "IX_Organisations_CreatorId",
                table: "Organisations");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Organisations");

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Organisations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
