using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kmc.API.Migrations
{
    /// <inheritdoc />
    public partial class FixIdentityKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventBookings_CityActivities_CityActivityActivityId",
                table: "EventBookings");

            migrationBuilder.DropIndex(
                name: "IX_EventBookings_CityActivityActivityId",
                table: "EventBookings");

            migrationBuilder.DropColumn(
                name: "CityActivityActivityId",
                table: "EventBookings");

            migrationBuilder.CreateIndex(
                name: "IX_EventBookings_ActivityId",
                table: "EventBookings",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventBookings_CityActivities_ActivityId",
                table: "EventBookings",
                column: "ActivityId",
                principalTable: "CityActivities",
                principalColumn: "ActivityId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventBookings_CityActivities_ActivityId",
                table: "EventBookings");

            migrationBuilder.DropIndex(
                name: "IX_EventBookings_ActivityId",
                table: "EventBookings");

            migrationBuilder.AddColumn<int>(
                name: "CityActivityActivityId",
                table: "EventBookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EventBookings_CityActivityActivityId",
                table: "EventBookings",
                column: "CityActivityActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventBookings_CityActivities_CityActivityActivityId",
                table: "EventBookings",
                column: "CityActivityActivityId",
                principalTable: "CityActivities",
                principalColumn: "ActivityId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
