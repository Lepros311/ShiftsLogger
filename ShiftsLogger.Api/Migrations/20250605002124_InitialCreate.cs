using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShiftsLogger.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<double>(type: "float", nullable: false),
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
                columns: new[] { "ShiftId", "Duration", "EndTime", "ShiftName", "StartTime", "WorkerId" },
                values: new object[,]
                {
                    { 1, 8.0, new DateTime(2023, 10, 15, 16, 0, 0, 0, DateTimeKind.Unspecified), "1st", new DateTime(2023, 10, 15, 8, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, 8.0, new DateTime(2023, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "2nd", new DateTime(2023, 10, 15, 16, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 3, 8.0, new DateTime(2023, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "2nd", new DateTime(2023, 10, 15, 16, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 4, 8.0, new DateTime(2023, 10, 15, 8, 0, 0, 0, DateTimeKind.Unspecified), "3rd", new DateTime(2023, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 5, 8.0, new DateTime(2023, 10, 15, 8, 0, 0, 0, DateTimeKind.Unspecified), "3rd", new DateTime(2023, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 6, 8.0, new DateTime(2023, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "2nd", new DateTime(2023, 10, 15, 16, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 7, 8.0, new DateTime(2023, 10, 15, 16, 0, 0, 0, DateTimeKind.Unspecified), "1st", new DateTime(2023, 10, 15, 8, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 8, 8.0, new DateTime(2023, 10, 15, 16, 0, 0, 0, DateTimeKind.Unspecified), "1st", new DateTime(2023, 10, 15, 8, 0, 0, 0, DateTimeKind.Unspecified), 4 }
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
