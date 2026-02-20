using Api.Databases.Repositories.BaseRepository;
using Api.Databases.Repositories.LigneCommandeRepository;
using Api.Domain.Entities;

namespace Api.Databases.UnitOfWork;


public interface IUnitOfWork: IDisposable{
    IBaseRepository<Client> Clients { get; }
    IBaseRepository<Categorie> Categories { get; }
    IBaseRepository<Commande> Commandes{ get; }
    IBaseRepository<Produit> Produits{ get; }
    ILigneCommandeRepository LigneCommandes{ get; }
    Task<int> SaveChangesAsync();
}