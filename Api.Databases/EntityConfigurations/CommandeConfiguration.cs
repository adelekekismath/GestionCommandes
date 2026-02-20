namespace Api.Databases.EntityConfigurations;

using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CommandeConfiguration : IEntityTypeConfiguration<Commande>
{
    public void Configure(EntityTypeBuilder<Commande> builder)
    {
        builder.HasMany(c => c.LignesCommande)
        .WithOne(c => c.Commande)
        .HasForeignKey(c => c.CommandeId)
        .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable(t => t.HasCheckConstraint(
            "CK_Commande_Statut_Valid",
            "STATUT IN ('EnAttente', 'EnCours', 'Livree', 'Annulee', 'Expediee')"

        ));
    }
}