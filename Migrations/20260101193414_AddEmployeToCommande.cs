using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManager.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeToCommande : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeId",
                table: "Commandes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Commandes_EmployeId",
                table: "Commandes",
                column: "EmployeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commandes_Employes_EmployeId",
                table: "Commandes",
                column: "EmployeId",
                principalTable: "Employes",
                principalColumn: "IdEmploye");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commandes_Employes_EmployeId",
                table: "Commandes");

            migrationBuilder.DropIndex(
                name: "IX_Commandes_EmployeId",
                table: "Commandes");

            migrationBuilder.DropColumn(
                name: "EmployeId",
                table: "Commandes");
        }
    }
}
