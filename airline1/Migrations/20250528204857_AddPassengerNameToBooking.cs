using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace airline1.Migrations
{
    /// <inheritdoc />
    public partial class AddPassengerNameToBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PassengerName",
                table: "Bookings",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassengerName",
                table: "Bookings");
        }
    }
}
