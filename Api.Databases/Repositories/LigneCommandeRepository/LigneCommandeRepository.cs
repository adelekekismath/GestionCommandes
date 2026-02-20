namespace Api.Databases.Repositories.LigneCommandeRepository;

using Api.Domain.Entities;
using Api.Databases.Contexts;
using Microsoft.EntityFrameworkCore;
using Api.ViewModel.DTOs;
using Api.Databases.Repositories.BaseRepository;

public class LigneCommandeRepository(
    AppDbContext context
) : BaseRepository<LigneCommande>(context), ILigneCommandeRepository
{
    public async Task<IEnumerable<TotalCommandeDto>> GetTotalCommandesByCommandeIdAsync()
    {
        var result = await DbSet
            .GroupBy(lc => lc.CommandeId)
            .Select(g => new TotalCommandeDto(
                g.Key, 
                (decimal)g.Sum(lc => lc.PrixUnitaire * lc.Quantite)
            ))
            .ToListAsync(); 

        return result;
    }
}