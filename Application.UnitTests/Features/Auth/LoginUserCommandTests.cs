using FluentAssertions;
using NSubstitute;
using Tap.Application.Features.Authentication;
using Tap.Application.Features.Authentication.LoginUser;
using Tap.Domain.Common.Services;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Users;

namespace Application.UnitTests.Features.Auth;

public class LoginUserCommandTests
{
    private static readonly LoginUserCommand Command = new("test@example.com", "validDDD1!");
    private readonly User _user =
        new("John Doe", Email.From("test@example.com"), "Password", UserRole.User);

    private readonly LoginUserCommandHandler _sut;

    private readonly IPasswordHashChecker _passwordHashChecker;
    private readonly IJwtProvider _jwtProvider;
    private readonly IUserRepository _userRepository;

    public LoginUserCommandTests()
    {
        _passwordHashChecker = Substitute.For<IPasswordHashChecker>();
        _jwtProvider = Substitute.For<IJwtProvider>();
        _userRepository = Substitute.For<IUserRepository>();

        _sut = new LoginUserCommandHandler(_passwordHashChecker, _jwtProvider, _userRepository);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_When_EmailIsNotValid()
    {
        // Arrange
        var command = Command with
        {
            Email = "invalid"
        };

        // Act
        var result = await _sut.Handle(command, default);

        // Assert
        result.Error.Should().Be(DomainErrors.Authentication.InvalidEmailOrPassword);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_When_UserNotFound()
    {
        // Arrange

        _userRepository.GetByEmailAsync(Command.Email, default).Returns(Maybe<User>.None);

        // Act
        var result = await _sut.Handle(Command, default);

        // Assert

        result.Error.Should().Be(DomainErrors.Authentication.InvalidEmailOrPassword);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_When_UserIsNotActive()
    {
        // Arrange
        _userRepository.GetByEmailAsync(Command.Email, default).Returns(Maybe<User>.From(_user));

        // Act
        var result = await _sut.Handle(Command, default);

        // Assert

        result.Error.Should().Be(DomainErrors.Authentication.AccountNotActive);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_When_PasswordIsNotValid()
    {
        // Arrange

        _user.Activate();

        _userRepository.GetByEmailAsync(Command.Email, default).Returns(Maybe<User>.From(_user));

        _passwordHashChecker.HashesMatch(Arg.Any<string>(), Arg.Any<string>()).Returns(false);

        // Act
        var result = await _sut.Handle(Command, default);

        // Assert

        result.Error.Should().Be(DomainErrors.Authentication.InvalidEmailOrPassword);
    }

    [Fact]
    public async Task Handle_Should_ReturnTokenResponse_When_CommandIsValid()
    {
        // Arrange

        _user.Activate();

        _userRepository.GetByEmailAsync(Command.Email, default).Returns(Maybe<User>.From(_user));

        _passwordHashChecker.HashesMatch(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        _jwtProvider.Create(Arg.Any<User>()).Returns("token");

        // Act
        var result = await _sut.Handle(Command, default);

        // Assert

        result.IsSuccess.Should().BeTrue();
    }
}
