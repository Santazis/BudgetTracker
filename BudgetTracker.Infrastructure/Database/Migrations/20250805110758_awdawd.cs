using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BudgetTracker.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class awdawd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "public",
                table: "Tag",
                keyColumn: "Id",
                keyValue: new Guid("07f76240-59f7-4b45-8b16-47fa23ebe7b0"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Tag",
                keyColumn: "Id",
                keyValue: new Guid("43cb31da-6ab7-45bc-9999-c4e60ba0ea25"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Tag",
                keyColumn: "Id",
                keyValue: new Guid("c6520f92-0b85-40fb-ac48-a7197aa9d75d"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Tag",
                keyColumn: "Id",
                keyValue: new Guid("cb072420-2885-45f8-824b-06ab8e252e6a"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Tag",
                keyColumn: "Id",
                keyValue: new Guid("ce16ca7c-7c2d-4148-bc2e-bf445c8de9f2"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "public",
                table: "Tag",
                columns: new[] { "Id", "CreatedAt", "IsSystem", "Name", "UserId" },
                values: new object[,]
                {
                    { new Guid("07f76240-59f7-4b45-8b16-47fa23ebe7b0"), new DateTime(2025, 8, 5, 10, 18, 52, 373, DateTimeKind.Utc).AddTicks(6713), true, "urgent", null },
                    { new Guid("43cb31da-6ab7-45bc-9999-c4e60ba0ea25"), new DateTime(2025, 8, 5, 10, 18, 52, 373, DateTimeKind.Utc).AddTicks(6467), true, "work", null },
                    { new Guid("c6520f92-0b85-40fb-ac48-a7197aa9d75d"), new DateTime(2025, 8, 5, 10, 18, 52, 373, DateTimeKind.Utc).AddTicks(6722), true, "trip", null },
                    { new Guid("cb072420-2885-45f8-824b-06ab8e252e6a"), new DateTime(2025, 8, 5, 10, 18, 52, 373, DateTimeKind.Utc).AddTicks(6714), true, "gift", null },
                    { new Guid("ce16ca7c-7c2d-4148-bc2e-bf445c8de9f2"), new DateTime(2025, 8, 5, 10, 18, 52, 373, DateTimeKind.Utc).AddTicks(6711), true, "personal", null }
                });
        }
    }
}
