namespace Api.Domain.Entities;

public class LigneCommande
{
    public int Id { get; set; }
    public int Quantite { get; set; }
    public float PrixUnitaire { get; set; }

    public int CommandeId { get; set; }
    public Commande Commande { get; set; } = default!;
    public int ProduitId { get; set; }
    public Produit Produit { get; set; } = default!;
}