﻿using FluentAssertions;
using NSubstitute;
using Tap.Application.Core.Abstractions.Cryptography;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Features.Users.CreateUser;
using Tap.Contracts.Features.Users;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives;
using Tap.Domain.Features.Users;

namespace Application.UnitTests;

public class CreateUserCommandTests
{
    private static readonly CreateUserCommand Command =
        new("John Doe", "test@example.com", "validDDD1!", UserRole.User);

    private readonly CreateUserCommandHandler _sut;

    private readonly IUserRepository _userRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly IPasswordHasher _passwordHasherMock;

    public CreateUserCommandTests()
    {
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _passwordHasherMock = Substitute.For<IPasswordHasher>();

        _sut = new CreateUserCommandHandler(
            _userRepositoryMock,
            _unitOfWorkMock,
            _passwordHasherMock
        );
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
        result.Error.Should().Be(DomainErrors.Email.InvalidFormat);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_When_EmailIsNotUnique()
    {
        // Arrange

        _userRepositoryMock.IsEmailUniqueAsync(Command.Email, default).Returns(true);
        _passwordHasherMock.HashPassword(Command.Password).Returns(Command.Password);

        // Act

        var result = await _sut.Handle(Command, default);

        // Assert

        result.Error.Should().Be(DomainErrors.User.DuplicateEmail);
    }

    [Fact]
    public async Task Handle_Should_ReturnUserResponse_When_CommandIsValid()
    {
        // Arrange

        _userRepositoryMock.IsEmailUniqueAsync(Command.Email, default).Returns(false);
        _passwordHasherMock.HashPassword(Command.Password).Returns(Command.Password);

        // Act

        var result = await _sut.Handle(Command, default);

        // Assert

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_CallUnitOfWork_When_CommandIsValid()
    {
        // Arrange

        _userRepositoryMock.IsEmailUniqueAsync(Command.Email, default).Returns(false);
        _passwordHasherMock.HashPassword(Command.Password).Returns(Command.Password);

        // Act

        await _sut.Handle(Command, default);

        // Assert

        await _unitOfWorkMock.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_Should_ReturnUserResponse_When_CommandIsValid_And_UserIsCreated()
    {
        // Arrange

        _userRepositoryMock.IsEmailUniqueAsync(Command.Email, default).Returns(false);
        _passwordHasherMock.HashPassword(Command.Password).Returns(Command.Password);

        // Act

        var result = await _sut.Handle(Command, default);

        // Assert

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<UserResponse>();
    }

    [Fact]
    public async Task Handle_Should_CallRepository_WhenEmailIsUnique()
    {
        // Arrange

        _userRepositoryMock.IsEmailUniqueAsync(Command.Email, default).Returns(false);
        _passwordHasherMock.HashPassword(Command.Password).Returns(Command.Password);

        // Act

        await _sut.Handle(Command, default);

        // Assert

        _userRepositoryMock.Received(1).Insert(Arg.Any<User>());
    }

    public static IEnumerable<object[]> PasswordTestData()
    {
        yield return new object[] { string.Empty, DomainErrors.Password.NullOrEmpty };
        yield return new object[] { "invalid", DomainErrors.Password.ShorterThanAllowed };
        yield return new object[] { "invalidddd", DomainErrors.Password.MissingUppercase };
        yield return new object[] { "INVALIDDDD", DomainErrors.Password.MissingLowercase };
        yield return new object[] { "invalidDDD", DomainErrors.Password.MissingDigit };
        yield return new object[] { "invalidDDD1", DomainErrors.Password.MissingNonAlphanumeric };
    }

    [Theory]
    [MemberData(nameof(PasswordTestData))]
    public async Task Handle_Should_ReturnError_ForInvalidPasswords(
        string password,
        Error expectedError
    )
    {
        // Arrange
        var command = Command with
        {
            Password = password
        };

        // Act
        var result = await _sut.Handle(command, default);

        // Assert
        result.Error.Should().Be(expectedError);
    }
}
