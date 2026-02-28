using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManager.Migrations
{
    /// <inheritdoc />
    public partial class AddPlatsCommandesRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "MontantBrut",
                table: "Ventes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Remise",
                table: "Ventes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "PlatsCommandes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommandeId = table.Column<int>(type: "int", nullable: false),
                    PlatId = table.Column<int>(type: "int", nullable: false),
                    Quantite = table.Column<int>(type: "int", nullable: false),
                    PrixUnitaire = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatsCommandes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatsCommandes_Commandes_CommandeId",
                        column: x => x.CommandeId,
                        principalTable: "Commandes",
                        principalColumn: "IdCommande",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlatsCommandes_Plats_PlatId",
                        column: x => x.PlatId,
                        principalTable: "Plats",
                        principalColumn: "IdPlat",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlatsCommandes_CommandeId",
                table: "PlatsCommandes",
                column: "CommandeId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatsCommandes_PlatId",
                table: "PlatsCommandes",
                column: "PlatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatsCommandes");

            migrationBuilder.DropColumn(
                name: "MontantBrut",
                table: "Ventes");

            migrationBuilder.DropColumn(
                name: "Remise",
                table: "Ventes");
        }
    }
}
