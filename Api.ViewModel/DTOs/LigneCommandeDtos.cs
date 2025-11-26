namespace Api.ViewModel.DTOs;

public record LigneCommandeCreateDto(
    int Quantite,
    int CommandeId,
    int ProduitId
);

public record LigneCommandeUpdateDto(
    int Quantite,
    int ProduitId
);