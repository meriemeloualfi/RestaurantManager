using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManager.Migrations
{
    /// <inheritdoc />
    public partial class ChangeToNomClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Clients_ClientId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_ClientId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Reservations");

            migrationBuilder.AddColumn<int>(
                name: "ClientIdClient",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomClient",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ClientIdClient",
                table: "Reservations",
                column: "ClientIdClient");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Clients_ClientIdClient",
                table: "Reservations",
                column: "ClientIdClient",
                principalTable: "Clients",
                principalColumn: "IdClient");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Clients_ClientIdClient",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_ClientIdClient",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ClientIdClient",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "NomClient",
                table: "Reservations");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ClientId",
                table: "Reservations",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Clients_ClientId",
                table: "Reservations",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "IdClient",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
