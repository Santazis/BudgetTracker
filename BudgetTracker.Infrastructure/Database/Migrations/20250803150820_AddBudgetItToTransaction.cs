using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetTracker.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddBudgetItToTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BudgetId",
                schema: "public",
                table: "Transaction",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_BudgetId",
                schema: "public",
                table: "Transaction",
                column: "BudgetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Budget_BudgetId",
                schema: "public",
                table: "Transaction",
                column: "BudgetId",
                principalSchema: "public",
                principalTable: "Budget",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Budget_BudgetId",
                schema: "public",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_BudgetId",
                schema: "public",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "BudgetId",
                schema: "public",
                table: "Transaction");
        }
    }
}
