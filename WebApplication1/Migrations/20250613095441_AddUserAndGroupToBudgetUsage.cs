using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetMobApp.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAndGroupToBudgetUsage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UsageDescription",
                table: "BudgetUsages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "BudgetUsages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "BudgetUsages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetUsages_GroupId",
                table: "BudgetUsages",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetUsages_UserId",
                table: "BudgetUsages",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetUsages_Groups_GroupId",
                table: "BudgetUsages",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetUsages_Users_UserId",
                table: "BudgetUsages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetUsages_Groups_GroupId",
                table: "BudgetUsages");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetUsages_Users_UserId",
                table: "BudgetUsages");

            migrationBuilder.DropIndex(
                name: "IX_BudgetUsages_GroupId",
                table: "BudgetUsages");

            migrationBuilder.DropIndex(
                name: "IX_BudgetUsages_UserId",
                table: "BudgetUsages");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "BudgetUsages");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BudgetUsages");

            migrationBuilder.AlterColumn<string>(
                name: "UsageDescription",
                table: "BudgetUsages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
