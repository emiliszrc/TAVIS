using Microsoft.EntityFrameworkCore.Migrations;

namespace LKOStest.Migrations
{
    public partial class ReviewersFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviewer_Reviews_ReviewId",
                table: "Reviewer");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviewer_Users_UserId",
                table: "Reviewer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviewer",
                table: "Reviewer");

            migrationBuilder.RenameTable(
                name: "Reviewer",
                newName: "Reviewers");

            migrationBuilder.RenameIndex(
                name: "IX_Reviewer_UserId",
                table: "Reviewers",
                newName: "IX_Reviewers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviewer_ReviewId",
                table: "Reviewers",
                newName: "IX_Reviewers_ReviewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviewers",
                table: "Reviewers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviewers_Reviews_ReviewId",
                table: "Reviewers",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviewers_Users_UserId",
                table: "Reviewers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviewers_Reviews_ReviewId",
                table: "Reviewers");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviewers_Users_UserId",
                table: "Reviewers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviewers",
                table: "Reviewers");

            migrationBuilder.RenameTable(
                name: "Reviewers",
                newName: "Reviewer");

            migrationBuilder.RenameIndex(
                name: "IX_Reviewers_UserId",
                table: "Reviewer",
                newName: "IX_Reviewer_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviewers_ReviewId",
                table: "Reviewer",
                newName: "IX_Reviewer_ReviewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviewer",
                table: "Reviewer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviewer_Reviews_ReviewId",
                table: "Reviewer",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviewer_Users_UserId",
                table: "Reviewer",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
