using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetMobApp.Migrations
{
    /// <inheritdoc />
    public partial class FixBudgetUsageUserGroupFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "BudgetDeposits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "BudgetDeposits",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetDeposits_GroupId",
                table: "BudgetDeposits",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetDeposits_UserId",
                table: "BudgetDeposits",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetDeposits_Groups_GroupId",
                table: "BudgetDeposits",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetDeposits_Users_UserId",
                table: "BudgetDeposits",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetDeposits_Groups_GroupId",
                table: "BudgetDeposits");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetDeposits_Users_UserId",
                table: "BudgetDeposits");

            migrationBuilder.DropIndex(
                name: "IX_BudgetDeposits_GroupId",
                table: "BudgetDeposits");

            migrationBuilder.DropIndex(
                name: "IX_BudgetDeposits_UserId",
                table: "BudgetDeposits");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "BudgetDeposits");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BudgetDeposits");
        }
    }
}
