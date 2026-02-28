using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManager.Migrations
{
    /// <inheritdoc />
    public partial class AddSeuilAlerte : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IngredientPlat");

            migrationBuilder.AddColumn<int>(
                name: "PlatIdPlat",
                table: "Ingredients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "SeuilAlerte",
                table: "Ingredients",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_PlatIdPlat",
                table: "Ingredients",
                column: "PlatIdPlat");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Plats_PlatIdPlat",
                table: "Ingredients",
                column: "PlatIdPlat",
                principalTable: "Plats",
                principalColumn: "IdPlat");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Plats_PlatIdPlat",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_PlatIdPlat",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "PlatIdPlat",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "SeuilAlerte",
                table: "Ingredients");

            migrationBuilder.CreateTable(
                name: "IngredientPlat",
                columns: table => new
                {
                    IngredientsIdIngredient = table.Column<int>(type: "int", nullable: false),
                    PlatsIdPlat = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientPlat", x => new { x.IngredientsIdIngredient, x.PlatsIdPlat });
                    table.ForeignKey(
                        name: "FK_IngredientPlat_Ingredients_IngredientsIdIngredient",
                        column: x => x.IngredientsIdIngredient,
                        principalTable: "Ingredients",
                        principalColumn: "IdIngredient",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IngredientPlat_Plats_PlatsIdPlat",
                        column: x => x.PlatsIdPlat,
                        principalTable: "Plats",
                        principalColumn: "IdPlat",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IngredientPlat_PlatsIdPlat",
                table: "IngredientPlat",
                column: "PlatsIdPlat");
        }
    }
}
