using Api.ViewModel.DTOs;
using FluentValidation;
using Api.Domain.Enums;

namespace Api.ViewModel.Validation;
public class CommandeCreateDtoValidator : AbstractValidator<CommandeCreateDto>
{
    
    public CommandeCreateDtoValidator()
    {
        RuleFor(x => x.ClientId)
            .GreaterThan(0).WithMessage("L'identifiant du client doit être supérieur à 0.");
    }
}

public class CommandeUpdateDtoValidator : AbstractValidator<CommandeUpdateDto>
{
    public CommandeUpdateDtoValidator()
    {
        RuleFor(x => x.Statut).Must(s => StatutCommandeHelper.StatutsValides.Contains(s))
            .WithMessage("Le statut doit être l'une des valeurs suivantes : EnAttente, EnCours, Livrée, Annulée ou Expédiée.");
    }
}
