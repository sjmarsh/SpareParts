using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpareParts.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedPartAttributeData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
            table: "PartAttribute",
            columns: new[] { "ID", "Name", "Description", "Value", "PartID" },
            values: new object[,]
            {
                        { 1, "Shape", "The Shape of the Part", "Round", 1 },
                        { 2, "Colour", "The Part Colour", "Orange", 1 },
                        { 3, "Shape", "The Shape of the Part", "Square", 2 },
                        { 4, "Colour", "The Part Colour", "Green", 2 },
                        { 5, "Size", "The Size of the Part", "Small", 3 },
                        { 6, "Colour", "The Part Colour", "Red", 3 }
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            for (int i = 1; i < 7; i++)
            {
                migrationBuilder.DeleteData(
                table: "PartAttribute",
                keyColumn: "ID",
                keyValue: i);
            }
        }
    }
}
