using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SpareParts.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Parts",
                columns: new[] { "ID", "Description", "EndDate", "Name", "Price", "StartDate", "Weight" },
                values: new object[,]
                {
                    { 1, "The first part", null, "Part 1", 10.1, new DateTime(2022, 5, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(6356), 1.1100000000000001 },
                    { 2, "The second part", null, "Part 2", 12.220000000000001, new DateTime(2022, 6, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(6546), 2.2200000000000002 },
                    { 3, "The third part", new DateTime(2023, 5, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(6562), "Part 3", 13.300000000000001, new DateTime(2022, 7, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(6560), 3.3300000000000001 },
                    { 4, "The fourth part", new DateTime(2022, 10, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(6578), "Part 4", 14.4, new DateTime(2021, 11, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(6576), 4.4400000000000004 }
                });

            migrationBuilder.InsertData(
                table: "InventoryItems",
                columns: new[] { "ID", "DateRecorded", "PartID", "Quantity" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 5, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(6966), 1, 11 },
                    { 2, new DateTime(2022, 6, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(7130), 1, 13 },
                    { 3, new DateTime(2022, 7, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(7143), 1, 5 },
                    { 4, new DateTime(2022, 6, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(7153), 2, 22 },
                    { 5, new DateTime(2022, 7, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(7164), 2, 16 },
                    { 6, new DateTime(2022, 8, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(7178), 2, 1 },
                    { 7, new DateTime(2022, 7, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(7188), 3, 33 },
                    { 8, new DateTime(2022, 8, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(7199), 3, 50 },
                    { 9, new DateTime(2022, 9, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(7209), 3, 40 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "ID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "ID",
                keyValue: 3);
        }
    }
}
