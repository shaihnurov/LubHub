using FluentValidation;

namespace LubHub.Application.Raffles.Commands.Validator;

/// <summary>
/// Validator for <see cref="FinishRaffleCommand"/>
/// </summary>
public class FinishRaffleCommandValidator : AbstractValidator<FinishRaffleCommand>
{
    /// <summary>
    /// Initializes validation rules for <see cref="FinishRaffleCommand"/>
    /// </summary>
    public FinishRaffleCommandValidator()
    {
        RuleFor(x => x.TwitchId).NotEmpty().WithMessage("Twitch ID is required.");
        RuleFor(x => x.RaffleId).GreaterThan(0).WithMessage("Raffle ID must be greater than zero.");
    }
}