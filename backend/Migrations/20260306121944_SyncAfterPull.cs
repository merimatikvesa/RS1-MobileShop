using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class SyncAfterPull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "BrandId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "BrandId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "SupplierId",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "BrandId", "Name" },
                values: new object[,]
                {
                    { 1, "Apple" },
                    { 2, "Samsung" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 1, "Smartphones" });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "SupplierId", "Address", "PhoneNumber", "SupplierName" },
                values: new object[] { 1, "Sarajevo, BiH", "+38761111222", "Main Supplier" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "BrandId", "CategoryId", "CreatedAt", "Model", "Price", "ProductName", "PromotionId", "SupplierId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "A2882", 999m, "iPhone 14", null, 1, null },
                    { 2, 1, 1, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "A2633", 799m, "iPhone 13", null, 1, null },
                    { 3, 2, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SM-S911B", 899m, "Samsung Galaxy S23", null, 1, null },
                    { 4, 2, 1, new DateTime(2025, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "SM-A546B", 449m, "Samsung Galaxy A54", null, 1, null }
                });
        }
    }
}
