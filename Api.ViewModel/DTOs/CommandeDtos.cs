using System.ComponentModel.DataAnnotations;
using Api.Domain.Enums;

namespace Api.ViewModel.DTOs;

public record CommandeCreateDto(
    [Required] int ClientId
);

public record CommandeUpdateDto(
    [Required] string Statut
);

public record TotalCommandeDto(
    int CommandeId,
    decimal Total
);
