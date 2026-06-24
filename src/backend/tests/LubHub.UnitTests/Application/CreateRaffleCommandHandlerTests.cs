using FluentAssertions;
using LubHub.Application.Common.Exceptions;
using LubHub.Application.Common.Interfaces;
using LubHub.Application.Raffles.Commands;
using LubHub.Domain.Entities;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace LubHub.UnitTests.Application;

public class CreateRaffleCommandHandlerTests
{
    private readonly IRaffleRepository _raffleRepository = Substitute.For<IRaffleRepository>();
    private readonly IStreamerRepository _streamerRepository = Substitute.For<IStreamerRepository>();
    private readonly CreateRaffleCommandHandler _handler;

    public CreateRaffleCommandHandlerTests()
    {
        _handler = new CreateRaffleCommandHandler(_raffleRepository, _streamerRepository);
    }

    [Fact]
    public async Task Handle_WhenStreamerExistsAndNoActiveRaffle_ShouldCreateRaffle()
    {
        var ct = TestContext.Current.CancellationToken;
        var streamer = Streamer.Create("twitch123", "TestStreamer", "test@test.com");
        _streamerRepository.GetByTwitchIdAsync("twitch123", ct).Returns(streamer);
        _raffleRepository.IsActiveRaffleExistsAsync(streamer.Id, ct).Returns(false);

        var command = new CreateRaffleCommand("twitch123", "Test Raffle");
        var result = await _handler.Handle(command, ct);

        result.Should().Be(0);
        await _raffleRepository.Received(1).AddAsync(Arg.Any<Raffle>(), ct);
    }

    [Fact]
    public async Task Handle_WhenStreamerNotFound_ShouldThrowNotFoundException()
    {
        var ct = TestContext.Current.CancellationToken;
        _streamerRepository.GetByTwitchIdAsync("twitch123", ct).ReturnsNull();

        var command = new CreateRaffleCommand("twitch123", "Test Raffle");
        var act = async () => await _handler.Handle(command, ct);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_WhenActiveRaffleExists_ShouldThrowBusinessRuleException()
    {
        var ct = TestContext.Current.CancellationToken;
        var streamer = Streamer.Create("twitch123", "TestStreamer", "test@test.com");
        _streamerRepository.GetByTwitchIdAsync("twitch123", ct).Returns(streamer);
        _raffleRepository.IsActiveRaffleExistsAsync(streamer.Id, ct).Returns(true);

        var command = new CreateRaffleCommand("twitch123", "Test Raffle");
        var act = async () => await _handler.Handle(command, ct);

        await act.Should().ThrowAsync<BusinessRuleException>();
    }
}