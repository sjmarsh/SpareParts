using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpareParts.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPartCategoryProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Parts",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 1,
                column: "DateRecorded",
                value: new DateTime(2023, 3, 6, 15, 26, 2, 649, DateTimeKind.Local).AddTicks(3892));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 2,
                column: "DateRecorded",
                value: new DateTime(2023, 4, 6, 15, 26, 2, 649, DateTimeKind.Local).AddTicks(3896));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 3,
                column: "DateRecorded",
                value: new DateTime(2023, 5, 6, 15, 26, 2, 649, DateTimeKind.Local).AddTicks(3898));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 4,
                column: "DateRecorded",
                value: new DateTime(2023, 4, 6, 15, 26, 2, 649, DateTimeKind.Local).AddTicks(3900));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 5,
                column: "DateRecorded",
                value: new DateTime(2023, 5, 6, 15, 26, 2, 649, DateTimeKind.Local).AddTicks(3902));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 6,
                column: "DateRecorded",
                value: new DateTime(2023, 6, 6, 15, 26, 2, 649, DateTimeKind.Local).AddTicks(3904));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 7,
                column: "DateRecorded",
                value: new DateTime(2023, 5, 6, 15, 26, 2, 649, DateTimeKind.Local).AddTicks(3906));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 8,
                column: "DateRecorded",
                value: new DateTime(2023, 6, 6, 15, 26, 2, 649, DateTimeKind.Local).AddTicks(3908));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 9,
                column: "DateRecorded",
                value: new DateTime(2023, 7, 6, 15, 26, 2, 649, DateTimeKind.Local).AddTicks(3911));

            migrationBuilder.UpdateData(
                table: "Parts",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "Category", "StartDate" },
                values: new object[] { 0, new DateTime(2023, 3, 6, 15, 26, 2, 649, DateTimeKind.Local).AddTicks(3648) });

            migrationBuilder.UpdateData(
                table: "Parts",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "Category", "StartDate" },
                values: new object[] { 3, new DateTime(2023, 4, 6, 15, 26, 2, 649, DateTimeKind.Local).AddTicks(3700) });

            migrationBuilder.UpdateData(
                table: "Parts",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "Category", "EndDate", "StartDate" },
                values: new object[] { null, new DateTime(2024, 3, 6, 15, 26, 2, 649, DateTimeKind.Local).AddTicks(3705), new DateTime(2023, 5, 6, 15, 26, 2, 649, DateTimeKind.Local).AddTicks(3703) });

            migrationBuilder.UpdateData(
                table: "Parts",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "Category", "EndDate", "StartDate" },
                values: new object[] { null, new DateTime(2023, 8, 6, 15, 26, 2, 649, DateTimeKind.Local).AddTicks(3712), new DateTime(2022, 9, 6, 15, 26, 2, 649, DateTimeKind.Local).AddTicks(3709) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Parts");

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 1,
                column: "DateRecorded",
                value: new DateTime(2023, 2, 27, 12, 55, 42, 498, DateTimeKind.Local).AddTicks(842));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 2,
                column: "DateRecorded",
                value: new DateTime(2023, 3, 27, 12, 55, 42, 498, DateTimeKind.Local).AddTicks(853));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 3,
                column: "DateRecorded",
                value: new DateTime(2023, 4, 27, 12, 55, 42, 498, DateTimeKind.Local).AddTicks(858));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 4,
                column: "DateRecorded",
                value: new DateTime(2023, 3, 27, 12, 55, 42, 498, DateTimeKind.Local).AddTicks(862));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 5,
                column: "DateRecorded",
                value: new DateTime(2023, 4, 27, 12, 55, 42, 498, DateTimeKind.Local).AddTicks(866));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 6,
                column: "DateRecorded",
                value: new DateTime(2023, 5, 27, 12, 55, 42, 498, DateTimeKind.Local).AddTicks(870));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 7,
                column: "DateRecorded",
                value: new DateTime(2023, 4, 27, 12, 55, 42, 498, DateTimeKind.Local).AddTicks(874));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 8,
                column: "DateRecorded",
                value: new DateTime(2023, 5, 27, 12, 55, 42, 498, DateTimeKind.Local).AddTicks(878));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 9,
                column: "DateRecorded",
                value: new DateTime(2023, 6, 27, 12, 55, 42, 498, DateTimeKind.Local).AddTicks(882));

            migrationBuilder.UpdateData(
                table: "Parts",
                keyColumn: "ID",
                keyValue: 1,
                column: "StartDate",
                value: new DateTime(2023, 2, 27, 12, 55, 42, 498, DateTimeKind.Local).AddTicks(470));

            migrationBuilder.UpdateData(
                table: "Parts",
                keyColumn: "ID",
                keyValue: 2,
                column: "StartDate",
                value: new DateTime(2023, 3, 27, 12, 55, 42, 498, DateTimeKind.Local).AddTicks(542));

            migrationBuilder.UpdateData(
                table: "Parts",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 2, 27, 12, 55, 42, 498, DateTimeKind.Local).AddTicks(551), new DateTime(2023, 4, 27, 12, 55, 42, 498, DateTimeKind.Local).AddTicks(548) });

            migrationBuilder.UpdateData(
                table: "Parts",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2023, 7, 27, 12, 55, 42, 498, DateTimeKind.Local).AddTicks(561), new DateTime(2022, 8, 27, 12, 55, 42, 498, DateTimeKind.Local).AddTicks(558) });
        }
    }
}
