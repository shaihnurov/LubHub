using FluentValidation;

namespace LubHub.Application.Raffles.Commands.Validator;

/// <summary>
/// Validator for <see cref="JoinRaffleCommand"/>
/// </summary>
public class JoinRaffleCommandValidator : AbstractValidator<JoinRaffleCommand>
{
    /// <summary>
    /// Initializes validation rules for <see cref="JoinRaffleCommand"/>
    /// </summary>
    public JoinRaffleCommandValidator()
    {
        RuleFor(x => x.TwitchUserId).NotEmpty().WithMessage("Twitch User ID is required.");

        RuleFor(x => x.DisplayName).NotEmpty().WithMessage("Display Name is required.");

        RuleFor(x => x.RaffleId).GreaterThan(0).WithMessage("Raffle ID must be greater than zero.");
    }
}