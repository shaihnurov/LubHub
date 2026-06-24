using FluentAssertions;
using LubHub.Application.Common.Exceptions;
using LubHub.Application.Common.Interfaces;
using LubHub.Application.Winners.Commands;
using LubHub.Domain.Entities;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace LubHub.UnitTests.Application;

public class DrawWinnerCommandHandlerTests
{
    private readonly IRaffleRepository _raffleRepository = Substitute.For<IRaffleRepository>();
    private readonly IStreamerRepository _streamerRepository = Substitute.For<IStreamerRepository>();
    private readonly IParticipantRepository _participantRepository = Substitute.For<IParticipantRepository>();
    private readonly IRedisService _redisService = Substitute.For<IRedisService>();
    private readonly IRaffleHubService _raffleHubService = Substitute.For<IRaffleHubService>();
    private readonly DrawWinnerCommandHandler _handler;

    public DrawWinnerCommandHandlerTests()
    {
        _handler = new DrawWinnerCommandHandler(_raffleRepository, _streamerRepository, _participantRepository, _redisService, _raffleHubService);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldReturnWinnerResponse()
    {
        var ct = TestContext.Current.CancellationToken;
        var streamer = Streamer.Create("twitch123", "TestStreamer", "test@test.com");
        var raffle = Raffle.Create(streamer.Id, "Test Raffle");
        raffle.Start();
        raffle.Finish();
        var participant = Participant.Create(raffle.Id, "winner456", "WinnerUser");

        _streamerRepository.GetByTwitchIdAsync("twitch123", ct).Returns(streamer);
        _raffleRepository.GetByIdAsync(1, ct).Returns(raffle);
        _redisService.GetRandomFromSetAsync("participants:1").Returns("winner456");
        _participantRepository.GetByTwitchUserIdAsync(raffle.Id, "winner456", ct).Returns(participant);

        var command = new DrawWinnerCommand("twitch123", 1);
        var result = await _handler.Handle(command, ct);

        result.TwitchUserId.Should().Be("winner456");
        result.DisplayName.Should().Be("WinnerUser");
        await _raffleRepository.Received(1).UpdateAsync(raffle, ct);
        await _raffleHubService.Received(1).SendWinnerAsync(raffle.Id, "winner456", "WinnerUser", ct);
    }

    [Fact]
    public async Task Handle_WhenStreamerNotFound_ShouldThrowNotFoundException()
    {
        var ct = TestContext.Current.CancellationToken;
        _streamerRepository.GetByTwitchIdAsync("twitch123", ct).ReturnsNull();

        var command = new DrawWinnerCommand("twitch123", 1);
        var act = async () => await _handler.Handle(command, ct);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_WhenRaffleNotFound_ShouldThrowNotFoundException()
    {
        var ct = TestContext.Current.CancellationToken;
        var streamer = Streamer.Create("twitch123", "TestStreamer", "test@test.com");
        _streamerRepository.GetByTwitchIdAsync("twitch123", ct).Returns(streamer);
        _raffleRepository.GetByIdAsync(1, ct).ReturnsNull();

        var command = new DrawWinnerCommand("twitch123", 1);
        var act = async () => await _handler.Handle(command, ct);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_WhenRaffleNotFinished_ShouldThrowBusinessRuleException()
    {
        var ct = TestContext.Current.CancellationToken;
        var streamer = Streamer.Create("twitch123", "TestStreamer", "test@test.com");
        var raffle = Raffle.Create(streamer.Id, "Test Raffle");
        raffle.Start();

        _streamerRepository.GetByTwitchIdAsync("twitch123", ct).Returns(streamer);
        _raffleRepository.GetByIdAsync(1, ct).Returns(raffle);

        var command = new DrawWinnerCommand("twitch123", 1);
        var act = async () => await _handler.Handle(command, ct);

        await act.Should().ThrowAsync<BusinessRuleException>().WithMessage("*not over*");
    }

    [Fact]
    public async Task Handle_WhenRaffleDoesNotBelongToStreamer_ShouldThrowBusinessRuleException()
    {
        var ct = TestContext.Current.CancellationToken;
        var streamer = Streamer.Create("twitch123", "TestStreamer", "test@test.com");
        var raffle = Raffle.Create(999, "Test Raffle");
        raffle.Start();
        raffle.Finish();

        _streamerRepository.GetByTwitchIdAsync("twitch123", ct).Returns(streamer);
        _raffleRepository.GetByIdAsync(1, ct).Returns(raffle);

        var command = new DrawWinnerCommand("twitch123", 1);
        var act = async () => await _handler.Handle(command, ct);

        await act.Should().ThrowAsync<BusinessRuleException>().WithMessage("*belong*");
    }
}