using LubHub.Domain.Entities;
using LubHub.Domain.Enums;
using LubHub.Domain.Exceptions;

namespace LubHub.UnitTests.Domain;

public class RaffleTests
{
    [Fact]
    public void Create_ShouldReturnRaffleWithPendingStatus()
    {
        var raffle = Raffle.Create(1, "Test Raffle");

        Assert.Equal(RaffleStatus.Pending, raffle.Status);
        Assert.Equal("Test Raffle", raffle.Title);
        Assert.Equal(1, raffle.StreamerId);
    }

    [Fact]
    public void Create_WhenTitleIsEmpty_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Raffle.Create(1, ""));
    }

    [Fact]
    public void Start_WhenPending_ShouldTransitionToActive()
    {
        var raffle = Raffle.Create(1, "Test Raffle");

        raffle.Start();

        Assert.Equal(RaffleStatus.Active, raffle.Status);
        Assert.NotNull(raffle.StartedAt);
    }

    [Fact]
    public void Start_WhenAlreadyActive_ShouldThrowRaffleDomainException()
    {
        var raffle = Raffle.Create(1, "Test Raffle");
        raffle.Start();

        Assert.Throws<RaffleDomainException>(() => raffle.Start());
    }

    [Fact]
    public void Start_WhenFinished_ShouldThrowRaffleDomainException()
    {
        var raffle = Raffle.Create(1, "Test Raffle");
        raffle.Start();
        raffle.Finish();

        Assert.Throws<RaffleDomainException>(() => raffle.Start());
    }

    [Fact]
    public void Finish_WhenActive_ShouldTransitionToFinished()
    {
        var raffle = Raffle.Create(1, "Test Raffle");
        raffle.Start();

        raffle.Finish();

        Assert.Equal(RaffleStatus.Finished, raffle.Status);
        Assert.NotNull(raffle.EndedAt);
    }

    [Fact]
    public void Finish_WhenPending_ShouldThrowRaffleDomainException()
    {
        var raffle = Raffle.Create(1, "Test Raffle");

        Assert.Throws<RaffleDomainException>(() => raffle.Finish());
    }

    [Fact]
    public void SetWinner_WhenFinished_ShouldAssignWinner()
    {
        var raffle = Raffle.Create(1, "Test Raffle");
        raffle.Start();
        raffle.Finish();
        var winner = Winner.Create(raffle.Id, 1, "twitch123", "TestUser");

        raffle.SetWinner(winner);

        Assert.NotNull(raffle.Winner);
        Assert.Equal("twitch123", raffle.Winner.TwitchUserId);
    }

    [Fact]
    public void SetWinner_WhenNotFinished_ShouldThrowRaffleDomainException()
    {
        var raffle = Raffle.Create(1, "Test Raffle");
        raffle.Start();
        var winner = Winner.Create(raffle.Id, 1, "twitch123", "TestUser");

        Assert.Throws<RaffleDomainException>(() => raffle.SetWinner(winner));
    }
}