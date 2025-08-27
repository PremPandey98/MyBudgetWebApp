using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetMobApp.Migrations
{
    /// <inheritdoc />
    public partial class AddSpendByToBudgetUsage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SpendBy",
                table: "BudgetUsages",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpendBy",
                table: "BudgetUsages");
        }
    }
}
