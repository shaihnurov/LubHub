using FluentValidation;

namespace LubHub.Application.Raffles.Commands;

/// <summary>
/// Validator for <see cref="CreateRaffleCommand"/>
/// </summary>
public class CreateRaffleCommandValidator : AbstractValidator<CreateRaffleCommand>
{
    public CreateRaffleCommandValidator()
    {
        RuleFor(x => x.TwitchId).NotEmpty().WithMessage("Twitch ID must not be empty");

        RuleFor(x => x.Title).NotEmpty().WithMessage("Title must not be empty")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");
    }
}