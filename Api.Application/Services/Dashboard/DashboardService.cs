using System.Threading.Tasks;
using Api.Databases.UnitOfWork;
using Api.ViewModel.DTOs; 


namespace Api.Application.Services.Dashboard;

public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public DashboardService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<DashboardStatsDto> GetKPIsAsync()
    {
        
        var clients = await _unitOfWork.Clients.CountAsync();
        var commandes = await _unitOfWork.Commandes.CountAsync();
        var produits = await _unitOfWork.Produits.CountAsync();
        var categories = await _unitOfWork.Categories.CountAsync();
        
        var commandesEnCours = await _unitOfWork.Commandes.CountAsync(
            c => c.Statut == "EnCours" || c.Statut == "EnAttente"
        );
        
        var produitsAlerteStock = await _unitOfWork.Produits.CountAsync(
            p => p.Stock <= 10 
        );

        return new DashboardStatsDto
        {
            TotalClients = clients,
            TotalCommandes = commandes,
            TotalProduits = produits,
            TotalCategories = categories,
            CommandesEnCours = commandesEnCours,
            ProduitsAlerteStock = produitsAlerteStock
        };
    }
}