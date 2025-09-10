using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixUniqueIssueSeats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Seats_ScreenId_SeatNumber",
                table: "Seats");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_ScreenId_RowNumber_SeatNumber",
                table: "Seats",
                columns: new[] { "ScreenId", "RowNumber", "SeatNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Seats_ScreenId_RowNumber_SeatNumber",
                table: "Seats");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_ScreenId_SeatNumber",
                table: "Seats",
                columns: new[] { "ScreenId", "SeatNumber" },
                unique: true);
        }
    }
}
