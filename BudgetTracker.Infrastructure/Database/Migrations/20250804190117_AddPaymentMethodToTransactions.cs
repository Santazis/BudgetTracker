using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetTracker.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentMethodToTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PaymentMethodId",
                schema: "public",
                table: "Transaction",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_PaymentMethodId",
                schema: "public",
                table: "Transaction",
                column: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_PaymentMethod_PaymentMethodId",
                schema: "public",
                table: "Transaction",
                column: "PaymentMethodId",
                principalSchema: "public",
                principalTable: "PaymentMethod",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_PaymentMethod_PaymentMethodId",
                schema: "public",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_PaymentMethodId",
                schema: "public",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                schema: "public",
                table: "Transaction");
        }
    }
}
