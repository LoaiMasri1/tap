﻿using FluentAssertions;
using NSubstitute;
using Tap.Application.Core.Abstractions.Common;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Features.Authentication;
using Tap.Application.Features.Discounts.CreateDiscount;
using Tap.Contracts.Features.Discounts;
using Tap.Domain.Common.ValueObjects;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Rooms;
using Tap.Domain.Features.Users;

namespace Application.UnitTests.Features.Discounts;

public class CreateDiscountCommandHandlerTests
{
    private readonly IRoomRepository _roomRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly IUserContext _userContextMock;
    private readonly IDateTime _dateTimeMock;

    private static readonly CreateDiscountCommand Command = new(1, "Name", "Description", 10);

    private readonly CreateDiscountCommandHandler _sut;

    public CreateDiscountCommandHandlerTests()
    {
        _roomRepositoryMock = Substitute.For<IRoomRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _userContextMock = Substitute.For<IUserContext>();
        _dateTimeMock = Substitute.For<IDateTime>();

        _dateTimeMock.UtcNow.Returns(DateTime.UtcNow.AddDays(-1));

        _sut = new CreateDiscountCommandHandler(
            _roomRepositoryMock,
            _unitOfWorkMock,
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
            .GetByIdWithDiscountsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Maybe<Room>.None);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.Error.Should().Be(DomainErrors.Room.NotFound);
    }

    [Fact]
    public async Task Handle_WhenRoomExists_ReturnsDiscountResponse()
    {
        _userContextMock.Role.Returns(UserRole.Admin);
        _roomRepositoryMock
            .GetByIdWithDiscountsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Room.Create(1, Money.Create(1, "USD"), RoomType.Single, 1, 1));

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<DiscountResponse>();
    }

    [Fact]
    public async Task Handle_WhenRoomExists_AddsDiscountToRoom()
    {
        _userContextMock.Role.Returns(UserRole.Admin);
        _roomRepositoryMock
            .GetByIdWithDiscountsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Room.Create(1, Money.Create(1, "USD"), RoomType.Single, 1, 1));
        _dateTimeMock.UtcNow.Returns(DateTime.UtcNow.AddDays(-1));

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<DiscountResponse>();

        var discount = result.Value;

        discount.DiscountPercentage.Should().Be(Command.DiscountPercentage);
    }

    [Fact]
    public async Task Handle_WhenRoomExists_CallsUnitOfWorkSaveChangesAsync()
    {
        _userContextMock.Role.Returns(UserRole.Admin);
        _roomRepositoryMock
            .GetByIdWithDiscountsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Room.Create(1, Money.Create(1, "USD"), RoomType.Single, 1, 1));

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<DiscountResponse>();

        await _unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
