using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Benteler.WorkPlan.Api.Migrations
{
    /// <inheritdoc />
    public partial class V1_0_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "WorkingDay",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingDay", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Year",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Year", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Year_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkingDaySection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkingDayId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingDaySection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkingDaySection_WorkingDay_WorkingDayId",
                        column: x => x.WorkingDayId,
                        principalTable: "WorkingDay",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CalenderWeek",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeekNumber = table.Column<int>(type: "int", nullable: false),
                    YearId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalenderWeek", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalenderWeek_Year_YearId",
                        column: x => x.YearId,
                        principalTable: "Year",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Day",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsHoliday = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkingDayId = table.Column<int>(type: "int", nullable: true),
                    CalenderWeekId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Day", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Day_CalenderWeek_CalenderWeekId",
                        column: x => x.CalenderWeekId,
                        principalTable: "CalenderWeek",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Day_WorkingDay_WorkingDayId",
                        column: x => x.WorkingDayId,
                        principalTable: "WorkingDay",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalenderWeek_YearId",
                table: "CalenderWeek",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_Day_CalenderWeekId",
                table: "Day",
                column: "CalenderWeekId");

            migrationBuilder.CreateIndex(
                name: "IX_Day_WorkingDayId",
                table: "Day",
                column: "WorkingDayId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingDaySection_WorkingDayId",
                table: "WorkingDaySection",
                column: "WorkingDayId");

            migrationBuilder.CreateIndex(
                name: "IX_Year_UserId",
                table: "Year",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Day");

            migrationBuilder.DropTable(
                name: "WorkingDaySection");

            migrationBuilder.DropTable(
                name: "CalenderWeek");

            migrationBuilder.DropTable(
                name: "WorkingDay");

            migrationBuilder.DropTable(
                name: "Year");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");
        }
    }
}
