using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpareParts.API.Migrations
{
    /// <inheritdoc />
    public partial class VersionUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 1,
                column: "DateRecorded",
                value: new DateTime(2011, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 2,
                column: "DateRecorded",
                value: new DateTime(2012, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 3,
                column: "DateRecorded",
                value: new DateTime(2013, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 4,
                column: "DateRecorded",
                value: new DateTime(2012, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 5,
                column: "DateRecorded",
                value: new DateTime(2012, 2, 23, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 6,
                column: "DateRecorded",
                value: new DateTime(2013, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 7,
                column: "DateRecorded",
                value: new DateTime(2013, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 8,
                column: "DateRecorded",
                value: new DateTime(2013, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 9,
                column: "DateRecorded",
                value: new DateTime(2013, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Parts",
                keyColumn: "ID",
                keyValue: 1,
                column: "StartDate",
                value: new DateTime(2011, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Parts",
                keyColumn: "ID",
                keyValue: 2,
                column: "StartDate",
                value: new DateTime(2012, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Parts",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2033, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2013, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Parts",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2034, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2014, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                column: "StartDate",
                value: new DateTime(2023, 3, 6, 15, 40, 30, 747, DateTimeKind.Local).AddTicks(9773));

            migrationBuilder.UpdateData(
                table: "Parts",
                keyColumn: "ID",
                keyValue: 2,
                column: "StartDate",
                value: new DateTime(2023, 4, 6, 15, 40, 30, 747, DateTimeKind.Local).AddTicks(9827));

            migrationBuilder.UpdateData(
                table: "Parts",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 3, 6, 15, 40, 30, 747, DateTimeKind.Local).AddTicks(9833), new DateTime(2023, 5, 6, 15, 40, 30, 747, DateTimeKind.Local).AddTicks(9831) });

            migrationBuilder.UpdateData(
                table: "Parts",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2023, 8, 6, 15, 40, 30, 747, DateTimeKind.Local).AddTicks(9840), new DateTime(2022, 9, 6, 15, 40, 30, 747, DateTimeKind.Local).AddTicks(9838) });
        }
    }
}
