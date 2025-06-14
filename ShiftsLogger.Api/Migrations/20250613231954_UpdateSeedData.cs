using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftsLogger.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Shifts",
                keyColumn: "ShiftId",
                keyValue: 1,
                column: "Date",
                value: new DateOnly(2025, 6, 15));

            migrationBuilder.UpdateData(
                table: "Shifts",
                keyColumn: "ShiftId",
                keyValue: 2,
                column: "Date",
                value: new DateOnly(2025, 6, 15));

            migrationBuilder.UpdateData(
                table: "Shifts",
                keyColumn: "ShiftId",
                keyValue: 3,
                column: "Date",
                value: new DateOnly(2025, 6, 14));

            migrationBuilder.UpdateData(
                table: "Shifts",
                keyColumn: "ShiftId",
                keyValue: 4,
                column: "Date",
                value: new DateOnly(2025, 6, 14));

            migrationBuilder.UpdateData(
                table: "Shifts",
                keyColumn: "ShiftId",
                keyValue: 5,
                column: "Date",
                value: new DateOnly(2025, 6, 13));

            migrationBuilder.UpdateData(
                table: "Shifts",
                keyColumn: "ShiftId",
                keyValue: 6,
                column: "Date",
                value: new DateOnly(2025, 6, 13));

            migrationBuilder.UpdateData(
                table: "Shifts",
                keyColumn: "ShiftId",
                keyValue: 7,
                column: "Date",
                value: new DateOnly(2025, 6, 14));

            migrationBuilder.UpdateData(
                table: "Shifts",
                keyColumn: "ShiftId",
                keyValue: 8,
                column: "Date",
                value: new DateOnly(2025, 6, 13));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Shifts",
                keyColumn: "ShiftId",
                keyValue: 1,
                column: "Date",
                value: new DateOnly(2023, 10, 15));

            migrationBuilder.UpdateData(
                table: "Shifts",
                keyColumn: "ShiftId",
                keyValue: 2,
                column: "Date",
                value: new DateOnly(2023, 10, 15));

            migrationBuilder.UpdateData(
                table: "Shifts",
                keyColumn: "ShiftId",
                keyValue: 3,
                column: "Date",
                value: new DateOnly(2023, 10, 15));

            migrationBuilder.UpdateData(
                table: "Shifts",
                keyColumn: "ShiftId",
                keyValue: 4,
                column: "Date",
                value: new DateOnly(2023, 10, 15));

            migrationBuilder.UpdateData(
                table: "Shifts",
                keyColumn: "ShiftId",
                keyValue: 5,
                column: "Date",
                value: new DateOnly(2023, 10, 15));

            migrationBuilder.UpdateData(
                table: "Shifts",
                keyColumn: "ShiftId",
                keyValue: 6,
                column: "Date",
                value: new DateOnly(2023, 10, 15));

            migrationBuilder.UpdateData(
                table: "Shifts",
                keyColumn: "ShiftId",
                keyValue: 7,
                column: "Date",
                value: new DateOnly(2023, 10, 15));

            migrationBuilder.UpdateData(
                table: "Shifts",
                keyColumn: "ShiftId",
                keyValue: 8,
                column: "Date",
                value: new DateOnly(2023, 10, 15));
        }
    }
}
