using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManager.Migrations
{
    /// <inheritdoc />
    public partial class FixTacheForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Statut",
                table: "Taches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Taches",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantite",
                table: "Ingredients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Taches_UserId",
                table: "Taches",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Taches_AspNetUsers_UserId",
                table: "Taches",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Taches_AspNetUsers_UserId",
                table: "Taches");

            migrationBuilder.DropIndex(
                name: "IX_Taches_UserId",
                table: "Taches");

            migrationBuilder.DropColumn(
                name: "Statut",
                table: "Taches");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Taches");

            migrationBuilder.DropColumn(
                name: "Quantite",
                table: "Ingredients");
        }
    }
}
