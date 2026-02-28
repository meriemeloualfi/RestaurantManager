using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManager.Migrations
{
    /// <inheritdoc />
    public partial class AddNumeroTableToCommande : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commandes_Clients_ClientId",
                table: "Commandes");

            migrationBuilder.AddColumn<string>(
                name: "NoteSpecial",
                table: "PlatsCommandes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Commandes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "NumeroTable",
                table: "Commandes",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Commandes_Clients_ClientId",
                table: "Commandes",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "IdClient");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commandes_Clients_ClientId",
                table: "Commandes");

            migrationBuilder.DropColumn(
                name: "NoteSpecial",
                table: "PlatsCommandes");

            migrationBuilder.DropColumn(
                name: "NumeroTable",
                table: "Commandes");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Commandes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Commandes_Clients_ClientId",
                table: "Commandes",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "IdClient",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
