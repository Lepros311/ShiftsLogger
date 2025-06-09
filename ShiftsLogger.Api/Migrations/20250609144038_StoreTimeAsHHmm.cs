using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShiftsLogger.Api.Migrations
{
    /// <inheritdoc />
    public partial class StoreTimeAsHHmm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    WorkerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.WorkerId);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    ShiftId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShiftName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "TIME(0)", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "TIME(0)", nullable: false),
                    Duration = table.Column<TimeOnly>(type: "time", nullable: false, computedColumnSql: "\r\n            CONVERT(TIME(0), \r\n                CASE \r\n                    WHEN EndTime < StartTime \r\n                        THEN DATEADD(MINUTE, DATEDIFF(MINUTE, \r\n                            CAST(StartTime AS DATETIME), \r\n                            DATEADD(DAY, 1, CAST(EndTime AS DATETIME))\r\n                        ), '00:00')\r\n                    ELSE DATEADD(MINUTE, DATEDIFF(MINUTE, \r\n                            CAST(StartTime AS DATETIME), \r\n                            CAST(EndTime AS DATETIME)\r\n                        ), '00:00')\r\n                END)"),
                    WorkerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.ShiftId);
                    table.ForeignKey(
                        name: "FK_Shifts_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "WorkerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Workers",
                columns: new[] { "WorkerId", "FirstName", "LastName", "Title" },
                values: new object[,]
                {
                    { 1, "Tom", "Foolery", "Laborer" },
                    { 2, "Sue", "Smith", "Laborer" },
                    { 3, "Doug", "Mitchell", "Foreman" },
                    { 4, "Mike", "Wilson", "Apprentice" }
                });

            migrationBuilder.InsertData(
                table: "Shifts",
                columns: new[] { "ShiftId", "Date", "EndTime", "ShiftName", "StartTime", "WorkerId" },
                values: new object[,]
                {
                    { 1, new DateOnly(2023, 10, 15), new TimeOnly(16, 0, 0), "1st", new TimeOnly(8, 0, 0), 1 },
                    { 2, new DateOnly(2023, 10, 15), new TimeOnly(0, 0, 0), "2nd", new TimeOnly(16, 0, 0), 1 },
                    { 3, new DateOnly(2023, 10, 15), new TimeOnly(0, 0, 0), "2nd", new TimeOnly(16, 0, 0), 2 },
                    { 4, new DateOnly(2023, 10, 15), new TimeOnly(8, 0, 0), "3rd", new TimeOnly(0, 0, 0), 2 },
                    { 5, new DateOnly(2023, 10, 15), new TimeOnly(0, 0, 0), "3rd", new TimeOnly(16, 0, 0), 3 },
                    { 6, new DateOnly(2023, 10, 15), new TimeOnly(0, 0, 0), "2nd", new TimeOnly(16, 0, 0), 3 },
                    { 7, new DateOnly(2023, 10, 15), new TimeOnly(16, 0, 0), "1st", new TimeOnly(8, 0, 0), 4 },
                    { 8, new DateOnly(2023, 10, 15), new TimeOnly(16, 0, 0), "1st", new TimeOnly(8, 0, 0), 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_WorkerId",
                table: "Shifts",
                column: "WorkerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "Workers");
        }
    }
}
