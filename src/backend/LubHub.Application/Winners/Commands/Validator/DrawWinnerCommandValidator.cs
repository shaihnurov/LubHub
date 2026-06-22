using FluentValidation;

namespace LubHub.Application.Winners.Commands.Validator;

/// <summary>
/// Validator for <see cref="DrawWinnerCommand"/>
/// </summary>
public class DrawWinnerCommandValidator : AbstractValidator<DrawWinnerCommand>
{
    public DrawWinnerCommandValidator()
    {
        RuleFor(x => x.RaffleId).GreaterThan(0).WithMessage("Raffle ID must be greater than zero.");
        RuleFor(x => x.TwitchId).NotEmpty().WithMessage("Twitch User ID is required.");
    }
}