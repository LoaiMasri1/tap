using FluentAssertions;
using NSubstitute;
using Tap.Application.Core.Abstractions.Common;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Features.Users.ActivateUser;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Users;

namespace Application.UnitTests.Features.Users;

public class ActivateUserCommandTests
{
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly IUserRepository _userRepositoryMock;
    private readonly IDateTime _dateTimeMock;

    private static readonly ActivateUserCommand Command = new("token");

    public ActivateUserCommandTests()
    {
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _dateTimeMock = Substitute.For<IDateTime>();
    }

    [Fact]
    public async Task Handle_Should_ReturnError_When_UserNotFound()
    {
        // Arrange
        _userRepositoryMock.GetByTokenAsync(Command.Token, default).Returns(Maybe<User>.None);

        var sut = new ActivateUserCommandHandler(
            _userRepositoryMock,
            _unitOfWorkMock,
            _dateTimeMock
        );

        // Act
        var result = await sut.Handle(Command, default);

        // Assert
        result.Error.Should().Be(DomainErrors.User.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_When_UserIsAlreadyActive()
    {
        // Arrange
        var user = new User(
            "John Doe",
            Email.From("test@example.com"),
            "validDDD1!",
            UserRole.User
        );

        user.Activate();

        _userRepositoryMock.GetByTokenAsync(Command.Token, default).Returns(user);

        var sut = new ActivateUserCommandHandler(
            _userRepositoryMock,
            _unitOfWorkMock,
            _dateTimeMock
        );

        // Act
        var result = await sut.Handle(Command, default);

        // Assert
        result.Error.Should().Be(DomainErrors.User.UserAllReadyActive);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_When_TokenExpired()
    {
        // Arrange
        var user = new User(
            "John Doe",
            Email.From("test@example.com"),
            "validDDD1!",
            UserRole.User,
            new Token("token", DateTime.UtcNow.AddDays(1))
        );

        var now = DateTime.UtcNow;

        _dateTimeMock.UtcNow.Returns(now.AddDays(2));

        _userRepositoryMock.GetByTokenAsync(Command.Token, default).Returns(user);

        var sut = new ActivateUserCommandHandler(
            _userRepositoryMock,
            _unitOfWorkMock,
            _dateTimeMock
        );

        // Act
        var result = await sut.Handle(Command, default);

        // Assert
        result.Error.Should().Be(DomainErrors.User.ActivationTokenExpired);
    }

    [Fact]
    public async Task Handle_Should_ActivateUser()
    {
        // Arrange
        var now = DateTime.UtcNow;

        var user = new User(
            "John Doe",
            Email.From("test@example.com"),
            "validDDD1!",
            UserRole.User,
            new Token("token", now.AddDays(1))
        );

        _dateTimeMock.UtcNow.Returns(now.AddDays(1));

        _userRepositoryMock.GetByTokenAsync(Command.Token, default).Returns(user);

        var sut = new ActivateUserCommandHandler(
            _userRepositoryMock,
            _unitOfWorkMock,
            _dateTimeMock
        );

        // Act
        var result = await sut.Handle(Command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}
