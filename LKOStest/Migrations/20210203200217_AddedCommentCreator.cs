using Microsoft.EntityFrameworkCore.Migrations;

namespace LKOStest.Migrations
{
    public partial class AddedCommentCreator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CreatorId",
                table: "Comments",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_CreatorId",
                table: "Comments",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_CreatorId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_CreatorId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Comments");
        }
    }
}
