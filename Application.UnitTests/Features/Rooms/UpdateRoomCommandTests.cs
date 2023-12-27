using FluentAssertions;
using NSubstitute;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Features.Authentication;
using Tap.Application.Features.Rooms.UpdateRoom;
using Tap.Contracts.Features.Rooms;
using Tap.Domain.Common.ValueObjects;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Rooms;
using Tap.Domain.Features.Users;

namespace Application.UnitTests.Features.Rooms;

public class UpdateRoomCommandTests
{
    private readonly IRoomRepository _roomRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly IUserIdentifierProvider _userIdentifierProviderMock;

    private static readonly UpdateRoomCommand Command =
        new(1, 1, 100, "EUR", RoomType.Single, 1, 1);

    private readonly UpdateRoomCommandHandler _sut;

    public UpdateRoomCommandTests()
    {
        _roomRepositoryMock = Substitute.For<IRoomRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _userIdentifierProviderMock = Substitute.For<IUserIdentifierProvider>();

        _sut = new UpdateRoomCommandHandler(
            _unitOfWorkMock,
            _userIdentifierProviderMock,
            _roomRepositoryMock
        );
    }

    [Fact]
    public async Task Handle_WhenUserIsNotAdmin_ReturnsUnauthorizedError()
    {
        _userIdentifierProviderMock.Role.Returns(UserRole.User);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.Error.Should().Be(DomainErrors.User.Unauthorized);
    }

    [Fact]
    public async Task Handle_WhenRoomDoesNotExist_ReturnsRoomNotFoundError()
    {
        _userIdentifierProviderMock.Role.Returns(UserRole.Admin);
        _roomRepositoryMock
            .GetByIdWithDiscountsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Maybe<Room>.None);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.Error.Should().Be(DomainErrors.Room.NotFound);
    }

    [Fact]
    public async Task Handle_WhenRoomExists_ReturnsRoomResponse()
    {
        _userIdentifierProviderMock.Role.Returns(UserRole.Admin);
        _roomRepositoryMock
            .GetByIdWithDiscountsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Room.Create(1, Money.Create(1, "USD"), RoomType.Single, 10, 1, false));

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<RoomResponse>();
    }

    [Fact]
    public async Task Handle_WhenRoomExists_UpdatesRoom()
    {
        _userIdentifierProviderMock.Role.Returns(UserRole.Admin);
        _roomRepositoryMock
            .GetByIdWithDiscountsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Room.Create(1, Money.Create(1, "USD"), RoomType.Single, 10, 1, false));

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<RoomResponse>();
    }

    [Fact]
    public async Task Handle_WhenRoomExists_SavesChanges()
    {
        _userIdentifierProviderMock.Role.Returns(UserRole.Admin);
        _roomRepositoryMock
            .GetByIdWithDiscountsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Room.Create(1, Money.Create(1, "USD"), RoomType.Single, 10, 1, false));

        await _sut.Handle(Command, CancellationToken.None);

        await _unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenRoomExistsAndNothingToUpdate_ReturnsNothingToUpdateError()
    {
        _userIdentifierProviderMock.Role.Returns(UserRole.Admin);
        _roomRepositoryMock
            .GetByIdWithDiscountsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Room.Create(1, Money.Create(1, "USD"), RoomType.Single, 10, 1, false));

        var result = await _sut.Handle(
            new UpdateRoomCommand(1, 1, 1, "USD", RoomType.Single, 10, 1),
            CancellationToken.None
        );

        result.Error.Should().Be(DomainErrors.Room.NothingToUpdate);
    }
}
