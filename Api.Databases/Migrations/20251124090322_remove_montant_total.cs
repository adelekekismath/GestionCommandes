using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Databases.Migrations
{
    /// <inheritdoc />
    public partial class remove_montant_total : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MontantTotal",
                table: "Commandes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MontantTotal",
                table: "Commandes",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
