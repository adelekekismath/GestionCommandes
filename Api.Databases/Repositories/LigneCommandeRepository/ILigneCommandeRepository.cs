namespace Api.Databases.Repositories.LigneCommandeRepository;

using Api.Databases.Repositories.BaseRepository;
using Api.Domain.Entities;
using Api.ViewModel.DTOs;

public interface ILigneCommandeRepository : IBaseRepository<LigneCommande>
{
    Task<IEnumerable<TotalCommandeDto>> GetTotalCommandesByCommandeIdAsync();
}