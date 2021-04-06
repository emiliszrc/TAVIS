using Microsoft.EntityFrameworkCore.Migrations;

namespace LKOStest.Migrations
{
    public partial class AddedContractTypesAndSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Organisations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContractType",
                table: "Contracts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Organisations");

            migrationBuilder.DropColumn(
                name: "ContractType",
                table: "Contracts");
        }
    }
}
