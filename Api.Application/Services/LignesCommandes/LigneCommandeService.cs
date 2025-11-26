namespace Api.Application.Services.LignesCommandes;

using Api.Domain.Entities;
using Api.Databases.Contexts;
using Microsoft.EntityFrameworkCore;
using Api.ViewModel.DTOs;
using Api.Domain.Entities;
using Api.Databases.UnitOfWork;
using Api.Application.Services.Produits;
using Api.Application.Services.Commandes;

public class LigneCommandeService: ILigneCommandeService
{
    private readonly IUnitOfWork _unityOfWork ;
    private static readonly string[] ImmutableStatuses = new[] { "Livree", "Annulee", "Expediee" };
    private readonly IProduitService produitService;

    public LigneCommandeService(IUnitOfWork unitOfWork){
        this.produitService = new ProduitService(unitOfWork);
        this._unityOfWork = unitOfWork;
    }

    public async Task<IEnumerable<LigneCommande>> GetAllAsync()
    {
        return await _unityOfWork.LigneCommandes.GetAllAsync();
    }

    public async Task<LigneCommande?> GetByIdAsync(int id)
    {
        return await _unityOfWork.LigneCommandes.GetByIdAsync(id);
    }

    public async Task<LigneCommande?> CreateAsync(LigneCommandeCreateDto dto)
    {
        var commande = await _unityOfWork.Commandes.GetByIdAsync(dto.CommandeId);
        if (commande is null)
            throw new KeyNotFoundException($"Commande avec l'ID {dto.CommandeId} introuvable.");

        if(ImmutableStatuses.Contains(commande.Statut))
            throw new InvalidOperationException($"Impossible d'ajouter une ligne à une commande avec le statut '{commande.Statut}'.");

        var produit = await _unityOfWork.Produits.GetByIdAsync(dto.ProduitId);
        if (produit is null)
            throw new KeyNotFoundException($"Produit avec l'ID {dto.ProduitId} introuvable.");
        
        var isStockSufficient = await produitService.IsStockSufficientAsync(dto.ProduitId, dto.Quantite);
        if (!isStockSufficient)
            throw new InvalidOperationException("Stock insuffisant pour le produit demandé.");

        var existedLine = await CommandeAndProduitExistInLigneCommandeAsync(dto.CommandeId, dto.ProduitId);
        if (existedLine is not null){
            existedLine.Quantite += dto.Quantite;
            await _unityOfWork.LigneCommandes.UpdateAsync(existedLine);
            await produitService.ReduceStockAsync(dto.ProduitId, dto.Quantite);
            return existedLine;
        }

        var ligneCommande = new LigneCommande
        {
            CommandeId = dto.CommandeId,
            ProduitId = dto.ProduitId,
            Quantite = dto.Quantite,
            PrixUnitaire = produit.Prix
        };

        await _unityOfWork.LigneCommandes.AddAsync(ligneCommande);
        await produitService.ReduceStockAsync(dto.ProduitId, dto.Quantite);
        await _unityOfWork.SaveChangesAsync();
        return ligneCommande;
    }

    

    public async Task<LigneCommande?> UpdateAsync(int id, LigneCommandeUpdateDto dto)
    {
        var ligneCommande = await _unityOfWork.LigneCommandes.GetByIdAsync(id);
        if (ligneCommande is null) return null;
        
        var commande = await _unityOfWork.Commandes.GetByIdAsync(ligneCommande.CommandeId);
        if (commande is null)
            throw new KeyNotFoundException($"Commande avec l'ID {ligneCommande.CommandeId} introuvable.");
        
        if(ImmutableStatuses.Contains(commande.Statut))
            throw new InvalidOperationException($"Impossible de modifier une ligne d'une commande avec le statut '{commande.Statut}'.");

        var produit = await _unityOfWork.Produits.GetByIdAsync(ligneCommande.ProduitId);
        if (produit is null)
            throw new KeyNotFoundException($"Produit avec l'ID {ligneCommande.ProduitId} introuvable.");

        
        var oldQuantite = ligneCommande.Quantite;
        var newQuantite = dto.Quantite;
        
        var differenceStock = newQuantite - oldQuantite;
        
        if (differenceStock == 0)
        {
            ligneCommande.PrixUnitaire = produit.Prix;
            await _unityOfWork.LigneCommandes.UpdateAsync(ligneCommande);
            await _unityOfWork.SaveChangesAsync();
            return ligneCommande;
        }
        else if (differenceStock > 0)
        {
            var isStockSufficient = await produitService.IsStockSufficientAsync(
                ligneCommande.ProduitId, 
                differenceStock 
            );
            await produitService.ReduceStockAsync(ligneCommande.ProduitId, differenceStock);
            
            if (!isStockSufficient)
            {
                throw new InvalidOperationException($"Stock insuffisant. Il manque {differenceStock} unité(s) pour le produit demandé.");
            }
        }
        else
        {
            await produitService.IncreaseStockAsync(ligneCommande.ProduitId, differenceStock);
        }
        
        
        ligneCommande.Quantite = newQuantite;
        ligneCommande.PrixUnitaire = produit.Prix; 
        
        
        var updatedLigneCommande = await _unityOfWork.LigneCommandes.UpdateAsync(ligneCommande);
        
        await _unityOfWork.SaveChangesAsync();
        
        return updatedLigneCommande;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        
        var ligneCommande = await _unityOfWork.LigneCommandes.GetByIdAsync(id);
        if (ligneCommande is null) return false;

        var commande = await _unityOfWork.Commandes.GetByIdAsync(ligneCommande.CommandeId);
        if (commande is null)
            throw new KeyNotFoundException($"Commande avec l'ID {ligneCommande.CommandeId} introuvable.");
        if(ImmutableStatuses.Contains(commande.Statut))
            throw new InvalidOperationException($"Impossible de supprimer une ligne d'une commande avec le statut '{commande.Statut}'.");
        
        await produitService.IncreaseStockAsync(ligneCommande.ProduitId, ligneCommande.Quantite);

        await _unityOfWork.LigneCommandes.DeleteAsync(ligneCommande);
        await _unityOfWork.SaveChangesAsync();
        return true;
    }
    
    public async Task<LigneCommande?> CommandeAndProduitExistInLigneCommandeAsync(int commandeId, int produitId)
    {
        return await _unityOfWork.LigneCommandes
            .FindAsync(lc => lc.CommandeId == commandeId && lc.ProduitId == produitId);
    }
}