using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Databases.Migrations
{
    /// <inheritdoc />
    public partial class update_enuù : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Commande_Statut_Valid",
                table: "Commandes");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Commande_Statut_Valid",
                table: "Commandes",
                sql: "STATUT IN ('EnAttente', 'EnCours', 'Livree', 'Annulee', 'Expediee')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Commande_Statut_Valid",
                table: "Commandes");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Commande_Statut_Valid",
                table: "Commandes",
                sql: "STATUT IN ('EnAttente', 'EnCours', 'Livrée', 'Annulée', 'Expédiée')");
        }
    }
}
