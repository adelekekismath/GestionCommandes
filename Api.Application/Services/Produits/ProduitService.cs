namespace Api.Application.Services.Produits;

using Api.Databases.Contexts;
using Microsoft.EntityFrameworkCore;
using Api.ViewModel.DTOs;
using Api.Domain.Entities;
using Api.Databases.UnitOfWork;
public class ProduitService( IUnitOfWork unitOfWork ) : IProduitService
{
    private readonly IUnitOfWork _unityOfWork = unitOfWork;

    public async Task<Produit?> CreateAsync(ProduitBaseDto dto)
    {
        
        var produit = new Produit
        {
            Nom = dto.Nom,
            Description = dto.Description,
            Prix = dto.Prix,
            Stock = dto.Stock,
            CategorieId = dto.CategorieId
        };

        var createdProduit = await _unityOfWork.Produits.AddAsync(produit);
        await _unityOfWork.SaveChangesAsync();
        return createdProduit;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var produit = await _unityOfWork.Produits.GetByIdAsync(id);
        if (produit == null) return false;

        await _unityOfWork.Produits.DeleteAsync(produit);
        await _unityOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Produit>> GetAllAsync()
    {
        return await _unityOfWork.Produits.GetAllAsync();
    }

    public async Task<Produit?> GetByIdAsync(int id)
    {
        return await _unityOfWork.Produits.GetByIdAsync(id);
    }

    public async Task<Produit?> UpdateAsync(int id, ProduitBaseDto dto)
    {
        var produit = await _unityOfWork.Produits.GetByIdAsync(id);
        if (produit == null) return null;

        produit.Nom = dto.Nom;
        produit.Description = dto.Description;
        produit.Prix = dto.Prix;
        produit.Stock = dto.Stock;
        produit.CategorieId = dto.CategorieId;

        var updatedProduit = await _unityOfWork.Produits.UpdateAsync(produit);
        await _unityOfWork.SaveChangesAsync();
        return updatedProduit;
    }

    public async Task<bool> IsStockSufficientAsync(int produitId, int quantite)
    {
        var produit = await _unityOfWork.Produits.GetByIdAsync(produitId);
        if (produit == null) return false;

        return produit.Stock >= quantite;
    }

    public async Task<bool> ReduceStockAsync(int produitId, int quantite)
    {
        var produit = await _unityOfWork.Produits.GetByIdAsync(produitId);
        if (produit == null || produit.Stock < quantite) return false;

        produit.Stock -= quantite;
        await _unityOfWork.Produits.UpdateAsync(produit);
        await _unityOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IncreaseStockAsync(int produitId, int quantite)
    {
        var produit = await _unityOfWork.Produits.GetByIdAsync(produitId);
        if (produit == null) return false;

        produit.Stock += quantite;
        await _unityOfWork.Produits.UpdateAsync(produit);
        await _unityOfWork.SaveChangesAsync();
        return true;
    }
}