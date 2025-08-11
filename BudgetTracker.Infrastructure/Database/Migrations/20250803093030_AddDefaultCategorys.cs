using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BudgetTracker.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultCategorys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "public",
                table: "Category",
                type: "uuid",
                nullable: true);

            migrationBuilder.InsertData(
                schema: "public",
                table: "Category",
                columns: new[] { "Id", "CreatedAt", "IsSystem", "Name", "Type", "UserId" },
                values: new object[,]
                {
                    { new Guid("1619d348-f694-4ee5-8b23-0e1325c834b8"), new DateTime(2025, 8, 3, 9, 30, 30, 341, DateTimeKind.Utc).AddTicks(3284), true, "Freelance", "Income", null },
                    { new Guid("3c9aca24-51f8-467d-a5ca-adcba6081089"), new DateTime(2025, 8, 3, 9, 30, 30, 341, DateTimeKind.Utc).AddTicks(3299), true, "Health", "Expense", null },
                    { new Guid("75e9948e-f798-4219-8976-b39af80dbe07"), new DateTime(2025, 8, 3, 9, 30, 30, 341, DateTimeKind.Utc).AddTicks(3285), true, "Food", "Expense", null },
                    { new Guid("7e14b446-b3c5-47b0-93ea-ac001ba12ff1"), new DateTime(2025, 8, 3, 9, 30, 30, 341, DateTimeKind.Utc).AddTicks(3020), true, "Salary", "Income", null },
                    { new Guid("9f1b5a18-4335-45c6-8de2-d252d9e71eb9"), new DateTime(2025, 8, 3, 9, 30, 30, 341, DateTimeKind.Utc).AddTicks(3300), true, "Education", "Expense", null },
                    { new Guid("a7787884-a503-4678-851e-1d126a3934ac"), new DateTime(2025, 8, 3, 9, 30, 30, 341, DateTimeKind.Utc).AddTicks(3292), true, "Transport", "Expense", null },
                    { new Guid("ae88da41-bccf-4889-9748-4539ad421980"), new DateTime(2025, 8, 3, 9, 30, 30, 341, DateTimeKind.Utc).AddTicks(3298), true, "Clothing", "Expense", null },
                    { new Guid("c3c395b3-c359-4cce-b81b-a48df0f3558d"), new DateTime(2025, 8, 3, 9, 30, 30, 341, DateTimeKind.Utc).AddTicks(3293), true, "Rent", "Expense", null },
                    { new Guid("cf58cf96-6694-4aa7-aa75-ed279c5379a8"), new DateTime(2025, 8, 3, 9, 30, 30, 341, DateTimeKind.Utc).AddTicks(3297), true, "Internet", "Expense", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_UserId",
                schema: "public",
                table: "Category",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_User_UserId",
                schema: "public",
                table: "Category",
                column: "UserId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_User_UserId",
                schema: "public",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Category_UserId",
                schema: "public",
                table: "Category");

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("1619d348-f694-4ee5-8b23-0e1325c834b8"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("3c9aca24-51f8-467d-a5ca-adcba6081089"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("75e9948e-f798-4219-8976-b39af80dbe07"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("7e14b446-b3c5-47b0-93ea-ac001ba12ff1"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("9f1b5a18-4335-45c6-8de2-d252d9e71eb9"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("a7787884-a503-4678-851e-1d126a3934ac"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("ae88da41-bccf-4889-9748-4539ad421980"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("c3c395b3-c359-4cce-b81b-a48df0f3558d"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("cf58cf96-6694-4aa7-aa75-ed279c5379a8"));

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "public",
                table: "Category");
        }
    }
}
