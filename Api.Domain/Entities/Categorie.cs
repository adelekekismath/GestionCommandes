namespace Api.Domain.Entities;

public class Categorie
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public ICollection<Produit> Produits { get; set; } = [];
}