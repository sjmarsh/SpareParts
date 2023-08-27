using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpareParts.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPartAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PartAttribute",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartAttribute", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PartAttribute_Parts_PartID",
                        column: x => x.PartID,
                        principalTable: "Parts",
                        principalColumn: "ID");
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_PartAttribute_PartID",
                table: "PartAttribute",
                column: "PartID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartAttribute");

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 1,
                column: "DateRecorded",
                value: new DateTime(2022, 5, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(6966));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 2,
                column: "DateRecorded",
                value: new DateTime(2022, 6, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(7130));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 3,
                column: "DateRecorded",
                value: new DateTime(2022, 7, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(7143));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 4,
                column: "DateRecorded",
                value: new DateTime(2022, 6, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(7153));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 5,
                column: "DateRecorded",
                value: new DateTime(2022, 7, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(7164));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 6,
                column: "DateRecorded",
                value: new DateTime(2022, 8, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(7178));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 7,
                column: "DateRecorded",
                value: new DateTime(2022, 7, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(7188));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 8,
                column: "DateRecorded",
                value: new DateTime(2022, 8, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(7199));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ID",
                keyValue: 9,
                column: "DateRecorded",
                value: new DateTime(2022, 9, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(7209));

            migrationBuilder.UpdateData(
                table: "Parts",
                keyColumn: "ID",
                keyValue: 1,
                column: "StartDate",
                value: new DateTime(2022, 5, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(6356));

            migrationBuilder.UpdateData(
                table: "Parts",
                keyColumn: "ID",
                keyValue: 2,
                column: "StartDate",
                value: new DateTime(2022, 6, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(6546));

            migrationBuilder.UpdateData(
                table: "Parts",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2023, 5, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(6562), new DateTime(2022, 7, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(6560) });

            migrationBuilder.UpdateData(
                table: "Parts",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2022, 10, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(6578), new DateTime(2021, 11, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(6576) });
        }
    }
}
