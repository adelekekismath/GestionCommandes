using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Databases.Migrations
{
    /// <inheritdoc />
    public partial class remove_numeroCommande_column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroCommande",
                table: "Commandes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NumeroCommande",
                table: "Commandes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
