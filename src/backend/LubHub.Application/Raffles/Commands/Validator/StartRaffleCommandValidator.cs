using FluentValidation;

namespace LubHub.Application.Raffles.Commands.Validator;

/// <summary>
/// Validator for <see cref="StartRaffleCommand"/>
/// </summary>
public class StartRaffleCommandValidator : AbstractValidator<StartRaffleCommand>
{
    /// <summary>
    /// Initializes validation rules for <see cref="StartRaffleCommand"/>
    /// </summary>
    public StartRaffleCommandValidator()
    {
        RuleFor(x => x.TwitchId).NotEmpty().WithMessage("Twitch ID is required.");
        RuleFor(x => x.RaffleId).GreaterThan(0).WithMessage("Raffle ID must be greater than zero.");
    }
}