namespace Api.Controllers;

using Api.ViewModel.DTOs;
using Api.Domain.Entities;
using Api.Databases.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Application.Services.LignesCommandes;
using Api.Application.Services.Commandes;
using Api.Application.Services.Produits;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LigneCommandeController : ControllerBase
{
    private readonly ILigneCommandeService _ligneCommandeService;
    private readonly ICommandeService _commandeService;
    private readonly IProduitService _produitService;

    public LigneCommandeController(ILigneCommandeService ligneCommandeService,
     ICommandeService commandeService,
     IProduitService produitService)
    {
        _ligneCommandeService = ligneCommandeService;
        _commandeService = commandeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var lignesCommandes = await _ligneCommandeService.GetAllAsync();
        return Ok(lignesCommandes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ligneCommande = await _ligneCommandeService.GetByIdAsync(id);
        if (ligneCommande == null)
        {
            return NotFound();
        }
        return Ok(ligneCommande);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LigneCommandeCreateDto dto)
    {   
        try
        {
            var createdLigneCommande = await _ligneCommandeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdLigneCommande.Id }, createdLigneCommande);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] LigneCommandeUpdateDto dto)
    {
        var updatedLigneCommande = await _ligneCommandeService.UpdateAsync(id, dto);
        if (updatedLigneCommande == null)
        {
            return NotFound();
        }
        return Ok(updatedLigneCommande);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _ligneCommandeService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}