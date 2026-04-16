using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueReservedSeatPerScreening : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScreeningId",
                table: "ReservedSeats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            // Backfill ScreeningId from the owning Reservation before the unique index
            // and FK are created — otherwise existing rows all collide at ScreeningId=0.
            migrationBuilder.Sql(@"
                UPDATE ""ReservedSeats"" AS rs
                SET ""ScreeningId"" = r.""ScreeningId""
                FROM ""Reservations"" AS r
                WHERE rs.""ReservationId"" = r.""Id"";
            ");

            migrationBuilder.CreateIndex(
                name: "IX_ReservedSeats_ScreeningId_SeatId",
                table: "ReservedSeats",
                columns: new[] { "ScreeningId", "SeatId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReservedSeats_Screenings_ScreeningId",
                table: "ReservedSeats",
                column: "ScreeningId",
                principalTable: "Screenings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservedSeats_Screenings_ScreeningId",
                table: "ReservedSeats");

            migrationBuilder.DropIndex(
                name: "IX_ReservedSeats_ScreeningId_SeatId",
                table: "ReservedSeats");

            migrationBuilder.DropColumn(
                name: "ScreeningId",
                table: "ReservedSeats");
        }
    }
}
