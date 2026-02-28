using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManager.Migrations
{
    /// <inheritdoc />
    public partial class MakeClientIdOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommandePlat");

            migrationBuilder.AddColumn<int>(
                name: "PlatIdPlat",
                table: "Commandes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Commandes_PlatIdPlat",
                table: "Commandes",
                column: "PlatIdPlat");

            migrationBuilder.AddForeignKey(
                name: "FK_Commandes_Plats_PlatIdPlat",
                table: "Commandes",
                column: "PlatIdPlat",
                principalTable: "Plats",
                principalColumn: "IdPlat");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commandes_Plats_PlatIdPlat",
                table: "Commandes");

            migrationBuilder.DropIndex(
                name: "IX_Commandes_PlatIdPlat",
                table: "Commandes");

            migrationBuilder.DropColumn(
                name: "PlatIdPlat",
                table: "Commandes");

            migrationBuilder.CreateTable(
                name: "CommandePlat",
                columns: table => new
                {
                    CommandesIdCommande = table.Column<int>(type: "int", nullable: false),
                    PlatsIdPlat = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandePlat", x => new { x.CommandesIdCommande, x.PlatsIdPlat });
                    table.ForeignKey(
                        name: "FK_CommandePlat_Commandes_CommandesIdCommande",
                        column: x => x.CommandesIdCommande,
                        principalTable: "Commandes",
                        principalColumn: "IdCommande",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommandePlat_Plats_PlatsIdPlat",
                        column: x => x.PlatsIdPlat,
                        principalTable: "Plats",
                        principalColumn: "IdPlat",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommandePlat_PlatsIdPlat",
                table: "CommandePlat",
                column: "PlatsIdPlat");
        }
    }
}
