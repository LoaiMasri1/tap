using FluentAssertions;
using NSubstitute;
using Tap.Application.Core.Abstractions.Cryptography;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Features.Authentication;
using Tap.Application.Features.Users.UpdateUser;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Users;

namespace Application.UnitTests.Features.Users;

public class UpdateUserCommandTests
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserIdentifierProvider _userIdentifierProvider;

    private static readonly UpdateUserCommand Command =
        new(1, "Loai", "Masri", "validDDD1!", "validDDD1!");

    private static readonly User User =
        new("John Doe", Email.From("test@example.com"), "Password", UserRole.User);

    private readonly UpdateUserCommandHandler _sut;

    public UpdateUserCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _userRepository = Substitute.For<IUserRepository>();
        _userIdentifierProvider = Substitute.For<IUserIdentifierProvider>();

        _sut = new UpdateUserCommandHandler(
            _unitOfWork,
            _passwordHasher,
            _userRepository,
            _userIdentifierProvider
        );
    }

    [Fact]
    public async Task Handle_WhenUserIsNotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        _userIdentifierProvider.Id.Returns(2);

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(DomainErrors.User.Unauthorized);
    }

    [Fact]
    public async Task Handle_WhenUserIsNotFound_ReturnsNotFound()
    {
        // Arrange
        _userIdentifierProvider.Id.Returns(1);
        _userRepository.GetByIdAsync(1, CancellationToken.None).Returns(Maybe<User>.None);

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(DomainErrors.User.NotFound);
    }

    [Fact]
    public async Task Handle_WhenPasswordIsNotProvided_ReturnsSuccess()
    {
        // Arrange
        _userIdentifierProvider.Id.Returns(1);
        _userRepository.GetByIdAsync(1, CancellationToken.None).Returns(Maybe<User>.From(User));

        _passwordHasher.HashPassword(Arg.Any<string>()).Returns(Command.Password);
        var command = Command with { Password = null, ConfirmPassword = null };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_WhenPasswordIsProvided_ReturnsSuccess()
    {
        // Arrange
        _userIdentifierProvider.Id.Returns(1);
        _userRepository.GetByIdAsync(1, default).Returns(Maybe<User>.From(User));

        _passwordHasher.HashPassword(Arg.Any<string>()).Returns(Command.Password);

        // Act
        var result = await _sut.Handle(Command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_WhenPasswordAndConfirmPasswordDoNotMatch_ReturnsPasswordsDoNotMatch()
    {
        // Arrange
        _userIdentifierProvider.Id.Returns(1);
        _userRepository.GetByIdAsync(1, default).Returns(Maybe<User>.From(User));

        var command = Command with { ConfirmPassword = "invalid" };

        // Act
        var result = await _sut.Handle(command, default);

        // Assert
        result.Error.Should().Be(DomainErrors.User.PasswordsDoNotMatch);
    }

    [Fact]
    public async Task Handle_WhenPasswordIsInvalid_ReturnsPasswordInvalid()
    {
        // Arrange
        _userIdentifierProvider.Id.Returns(1);
        _userRepository.GetByIdAsync(1, default).Returns(Maybe<User>.From(User));

        var command = Command with { Password = "invalid", ConfirmPassword = "invalid" };

        var passwordResult = Password.Create(command.Password);

        // Act
        var result = await _sut.Handle(command, default);

        // Assert
        result.Error.Should().Be(passwordResult.Error);
    }

    [Fact]
    public async Task Handle_WhenPasswordIsTheSameAsTheOldOne_ReturnsCannotChangePassword()
    {
        // Arrange
        _userIdentifierProvider.Id.Returns(1);
        _userRepository.GetByIdAsync(1, default).Returns(Maybe<User>.From(User));
        _passwordHasher.HashPassword(Arg.Any<string>()).Returns("validDDD1!");

        var command = Command with { Password = "validDDD1!", ConfirmPassword = "validDDD1!" };

        User.UpdatePassword(Command.Password!);

        // Act
        var result = await _sut.Handle(command, default);

        // Assert
        result.Error.Should().Be(DomainErrors.User.CannotChangePassword);
    }
}
