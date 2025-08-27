using Microsoft.EntityFrameworkCore.Migrations;

namespace BudgetMobApp.Migrations
{
    public partial class AddUserAndGroupToBudgetDepositAndUsage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "BudgetDeposit",
                type: "int",
                nullable: true);
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "BudgetDeposit",
                type: "int",
                nullable: true);
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "BudgetUsage",
                type: "int",
                nullable: true);
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "BudgetUsage",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BudgetDeposit");
            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "BudgetDeposit");
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BudgetUsage");
            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "BudgetUsage");
        }
    }
}
