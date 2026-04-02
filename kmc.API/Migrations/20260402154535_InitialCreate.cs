using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kmc.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CityActivities",
                columns: table => new
                {
                    ActivityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Venue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxParticipants = table.Column<int>(type: "int", nullable: false),
                    OrganizerId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityActivities", x => x.ActivityId);
                });

            migrationBuilder.CreateTable(
                name: "EventBookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    ResidentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CityActivityActivityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventBookings", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_EventBookings_CityActivities_CityActivityActivityId",
                        column: x => x.CityActivityActivityId,
                        principalTable: "CityActivities",
                        principalColumn: "ActivityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventBookings_CityActivityActivityId",
                table: "EventBookings",
                column: "CityActivityActivityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventBookings");

            migrationBuilder.DropTable(
                name: "CityActivities");
        }
    }
}
