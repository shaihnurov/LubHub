using FluentAssertions;
using LubHub.Application.Common.Exceptions;
using LubHub.Application.Common.Interfaces;
using LubHub.Application.Common.Messages;
using LubHub.Application.Raffles.Commands;
using LubHub.Domain.Entities;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace LubHub.UnitTests.Application;

public class JoinRaffleCommandHandlerTests
{
    private readonly IRaffleRepository _raffleRepository = Substitute.For<IRaffleRepository>();
    private readonly IRedisService _redisService = Substitute.For<IRedisService>();
    private readonly IEventBus _eventBus = Substitute.For<IEventBus>();
    private readonly JoinRaffleCommandHandler _handler;

    public JoinRaffleCommandHandlerTests()
    {
        _handler = new JoinRaffleCommandHandler(_raffleRepository, _redisService, _eventBus);
    }

    [Fact]
    public async Task Handle_WhenRaffleActiveAndNotDuplicate_ShouldPublishMessage()
    {
        var ct = TestContext.Current.CancellationToken;
        var raffle = Raffle.Create(1, "Test Raffle");
        raffle.Start();

        _raffleRepository.GetByIdAsync(1, ct).Returns(raffle);
        _redisService.AddToSetAsync("participants:1", "twitch123").Returns(true);

        var command = new JoinRaffleCommand(1, "twitch123", "TestUser");
        await _handler.Handle(command, ct);

        await _eventBus.Received(1).PublishAsync(
            Arg.Is<JoinRaffleMessage>(m =>
                m.RaffleId == 1 &&
                m.TwitchUserId == "twitch123" &&
                m.DisplayName == "TestUser"),
            ct);
    }

    [Fact]
    public async Task Handle_WhenRaffleNotFound_ShouldThrowNotFoundException()
    {
        var ct = TestContext.Current.CancellationToken;
        _raffleRepository.GetByIdAsync(1, ct).ReturnsNull();

        var command = new JoinRaffleCommand(1, "twitch123", "TestUser");
        var act = async () => await _handler.Handle(command, ct);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_WhenRaffleNotActive_ShouldThrowBusinessRuleException()
    {
        var ct = TestContext.Current.CancellationToken;
        var raffle = Raffle.Create(1, "Test Raffle");
        _raffleRepository.GetByIdAsync(1, ct).Returns(raffle);

        var command = new JoinRaffleCommand(1, "twitch123", "TestUser");
        var act = async () => await _handler.Handle(command, ct);

        await act.Should().ThrowAsync<BusinessRuleException>().WithMessage("*active*");
    }

    [Fact]
    public async Task Handle_WhenParticipantAlreadyJoined_ShouldThrowBusinessRuleException()
    {
        var ct = TestContext.Current.CancellationToken;
        var raffle = Raffle.Create(1, "Test Raffle");
        raffle.Start();

        _raffleRepository.GetByIdAsync(1, ct).Returns(raffle);
        _redisService.AddToSetAsync("participants:1", "twitch123").Returns(false);

        var command = new JoinRaffleCommand(1, "twitch123", "TestUser");
        var act = async () => await _handler.Handle(command, ct);

        await act.Should().ThrowAsync<BusinessRuleException>().WithMessage("*already joined*");
    }
}