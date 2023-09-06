using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpareParts.API.Migrations
{
    /// <inheritdoc />
    public partial class EnsurePartCategoryStoredAsString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Parts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 1,
                column: "DateRecorded",
                value: new DateTime(2023, 3, 6, 15, 40, 30, 748, DateTimeKind.Local).AddTicks(424));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 2,
                column: "DateRecorded",
                value: new DateTime(2023, 4, 6, 15, 40, 30, 748, DateTimeKind.Local).AddTicks(439));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 3,
                column: "DateRecorded",
                value: new DateTime(2023, 5, 6, 15, 40, 30, 748, DateTimeKind.Local).AddTicks(441));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 4,
                column: "DateRecorded",
                value: new DateTime(2023, 4, 6, 15, 40, 30, 748, DateTimeKind.Local).AddTicks(443));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 5,
                column: "DateRecorded",
                value: new DateTime(2023, 5, 6, 15, 40, 30, 748, DateTimeKind.Local).AddTicks(446));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 6,
                column: "DateRecorded",
                value: new DateTime(2023, 6, 6, 15, 40, 30, 748, DateTimeKind.Local).AddTicks(448));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 7,
                column: "DateRecorded",
                value: new DateTime(2023, 5, 6, 15, 40, 30, 748, DateTimeKind.Local).AddTicks(450));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 8,
                column: "DateRecorded",
                value: new DateTime(2023, 6, 6, 15, 40, 30, 748, DateTimeKind.Local).AddTicks(452));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 9,
                column: "DateRecorded",
                value: new DateTime(2023, 7, 6, 15, 40, 30, 748, DateTimeKind.Local).AddTicks(454));

            migrationBuilder.UpdateData(
                table: "Parts",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "Category", "StartDate" },
                values: new object[] { "Electronic", new DateTime(2023, 3, 6, 15, 40, 30, 747, DateTimeKind.Local).AddTicks(9773) });

            migrationBuilder.UpdateData(
                table: "Parts",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "Category", "StartDate" },
                values: new object[] { "Miscellaneous", new DateTime(2023, 4, 6, 15, 40, 30, 747, DateTimeKind.Local).AddTicks(9827) });

            migrationBuilder.UpdateData(
                table: "Parts",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "Category", "EndDate", "StartDate" },
                values: new object[] { null, new DateTime(2024, 3, 6, 15, 40, 30, 747, DateTimeKind.Local).AddTicks(9833), new DateTime(2023, 5, 6, 15, 40, 30, 747, DateTimeKind.Local).AddTicks(9831) });

            migrationBuilder.UpdateData(
                table: "Parts",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "Category", "EndDate", "StartDate" },
                values: new object[] { null, new DateTime(2023, 8, 6, 15, 40, 30, 747, DateTimeKind.Local).AddTicks(9840), new DateTime(2022, 9, 6, 15, 40, 30, 747, DateTimeKind.Local).AddTicks(9838) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Category",
                table: "Parts",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
    }
}
