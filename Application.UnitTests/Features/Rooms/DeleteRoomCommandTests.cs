using FluentAssertions;
using NSubstitute;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Features.Authentication;
using Tap.Application.Features.Rooms.DeleteRoom;
using Tap.Domain.Common.ValueObjects;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Rooms;
using Tap.Domain.Features.Users;

namespace Application.UnitTests.Features.Rooms;

public class DeleteRoomCommandTests
{
    private readonly IRoomRepository _roomRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly IUserContext _userContextMock;

    private static readonly DeleteRoomCommand Command = new(1);

    private readonly DeleteRoomCommandHandler _sut;

    public DeleteRoomCommandTests()
    {
        _roomRepositoryMock = Substitute.For<IRoomRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _userContextMock = Substitute.For<IUserContext>();

        _sut = new DeleteRoomCommandHandler(
            _unitOfWorkMock,
            _roomRepositoryMock,
            _userContextMock
        );
    }

    [Fact]
    public async Task Handle_WhenUserIsNotAdmin_ReturnsUnauthorizedError()
    {
        _userContextMock.Role.Returns(UserRole.User);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.Error.Should().Be(DomainErrors.User.Unauthorized);
    }

    [Fact]
    public async Task Handle_WhenRoomDoesNotExist_ReturnsRoomNotFoundError()
    {
        _userContextMock.Role.Returns(UserRole.Admin);
        _roomRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Maybe<Room>.None);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.Error.Should().Be(DomainErrors.Room.NotFound);
    }

    [Fact]
    public async Task Handle_WhenRoomExists_ReturnsSuccess()
    {
        _userContextMock.Role.Returns(UserRole.Admin);
        _roomRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Room.Create(1, Money.Create(1, "USD"), RoomType.Cabana));

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenRoomExists_DeletesRoom()
    {
        _userContextMock.Role.Returns(UserRole.Admin);
        _roomRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Room.Create(1, Money.Create(1, "USD"), RoomType.Cabana));

        var result = await _sut.Handle(Command, CancellationToken.None);

        _roomRepositoryMock.Received(1).Remove(Arg.Any<Room>());
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenRoomExists_SavesChanges()
    {
        _userContextMock.Role.Returns(UserRole.Admin);
        _roomRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Room.Create(1, Money.Create(1, "USD"), RoomType.Cabana));

        await _sut.Handle(Command, CancellationToken.None);

        await _unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
