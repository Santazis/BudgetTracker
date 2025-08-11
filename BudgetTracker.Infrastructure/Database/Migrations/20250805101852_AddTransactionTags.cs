using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BudgetTracker.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_PaymentMethod_PaymentMethodId",
                schema: "public",
                table: "Transaction");

            migrationBuilder.CreateTable(
                name: "Tag",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tag_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionTag",
                schema: "public",
                columns: table => new
                {
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTag", x => new { x.TransactionId, x.TagId });
                    table.ForeignKey(
                        name: "FK_TransactionTag_Tag_TagId",
                        column: x => x.TagId,
                        principalSchema: "public",
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionTag_Transaction_TransactionId",
                        column: x => x.TransactionId,
                        principalSchema: "public",
                        principalTable: "Transaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Tag_UserId",
                schema: "public",
                table: "Tag",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTag_TagId",
                schema: "public",
                table: "TransactionTag",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_PaymentMethod_PaymentMethodId",
                schema: "public",
                table: "Transaction",
                column: "PaymentMethodId",
                principalSchema: "public",
                principalTable: "PaymentMethod",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_PaymentMethod_PaymentMethodId",
                schema: "public",
                table: "Transaction");

            migrationBuilder.DropTable(
                name: "TransactionTag",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Tag",
                schema: "public");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_PaymentMethod_PaymentMethodId",
                schema: "public",
                table: "Transaction",
                column: "PaymentMethodId",
                principalSchema: "public",
                principalTable: "PaymentMethod",
                principalColumn: "Id");
        }
    }
}
