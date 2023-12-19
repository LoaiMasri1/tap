using FluentAssertions;
using NSubstitute;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Features.Authentication;
using Tap.Application.Features.Hotels.CreateHotel;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Cities;
using Tap.Domain.Features.Users;

namespace Application.UnitTests.Features.Hotels;

public class CreateHotelCommandTests
{
    private readonly ICityRepository _cityRepositoryMock;
    private readonly IUserRepository _userRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly IUserIdentifierProvider _userIdentifierProviderMock;

    private static readonly CreateHotelCommand Command =
        new("Hotel Name", "Hotel Description", 34.4, 35.6, 1);

    private readonly CreateHotelCommandHandler _sut;

    public CreateHotelCommandTests()
    {
        _cityRepositoryMock = Substitute.For<ICityRepository>();
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _userIdentifierProviderMock = Substitute.For<IUserIdentifierProvider>();

        _sut = new CreateHotelCommandHandler(
            _cityRepositoryMock,
            _userRepositoryMock,
            _unitOfWorkMock,
            _userIdentifierProviderMock
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
    public async Task Handle_WhenCityDoesNotExist_ReturnsCityNotFoundError()
    {
        _userIdentifierProviderMock.Role.Returns(UserRole.Admin);
        _cityRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Maybe<City>.None);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.Error.Should().Be(DomainErrors.City.NotFound);
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ReturnsUserNotFoundError()
    {
        // Arrange
        _userIdentifierProviderMock.Role.Returns(UserRole.Admin);
        _cityRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(new City(Command.Name, Command.Description, "PS"));
        _userRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Maybe<User>.None);

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(DomainErrors.User.NotFound);
    }

    // create Location
    [Fact]
    public async Task Handle_WhenLatitudeOutOfRange_ReturnsLatitudeOutOfRangeError()
    {
        // Arrange
        _userIdentifierProviderMock.Role.Returns(UserRole.Admin);
        _cityRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), default)
            .Returns(new City(Command.Name, Command.Description, "PS"));

        _userRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), default)
            .Returns(new User("test", Email.From("test@example.com"), "password", UserRole.Admin));

        var command = Command with { Latitude = 100 };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(DomainErrors.Location.LatitudeOutOfRange);
    }

    [Fact]
    public async Task Handle_WhenLongitudeOutOfRange_ReturnsLongitudeOutOfRangeError()
    {
        // Arrange
        var command = Command with
        {
            Longitude = 200
        };

        _userIdentifierProviderMock.Role.Returns(UserRole.Admin);
        _cityRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), default)
            .Returns(new City(command.Name, command.Description, "PS"));

        _userRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), default)
            .Returns(new User("test", Email.From("test@example.com"), "password", UserRole.Admin));

        // Act
        var result = await _sut.Handle(command, default);

        // Assert
        result.Error.Should().Be(DomainErrors.Location.LongitudeOutOfRange);
    }

    [Fact]
    public async Task Handle_WhenLatitudeIsNull_ReturnsNullLatitudeError()
    {
        // Arrange
        var command = Command with
        {
            Latitude = double.NaN
        };

        _userIdentifierProviderMock.Role.Returns(UserRole.Admin);
        _cityRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(new City(command.Name, command.Description, "PS"));
        _userRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), default)
            .Returns(new User("test", Email.From("test@example.com"), "password", UserRole.Admin));

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(DomainErrors.Location.NullLatitude);
    }

    [Fact]
    public async Task Handle_WhenLongitudeIsNull_ReturnsNullLongitudeError()
    {
        // Arrange
        var command = Command with
        {
            Longitude = double.NaN
        };

        _userIdentifierProviderMock.Role.Returns(UserRole.Admin);
        _cityRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(new City(command.Name, command.Description, "PS"));

        _userRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), default)
            .Returns(new User("test", Email.From("test@example.com"), "password", UserRole.Admin));

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(DomainErrors.Location.NullLongitude);
    }

    [Fact]
    public async Task Handle_WhenLocationIsNull_ReturnsNullOrEmptyError()
    {
        // Arrange
        _userIdentifierProviderMock.Role.Returns(UserRole.Admin);
        _cityRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(new City(Command.Name, Command.Description, "PS"));
        _userRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), default)
            .Returns(new User("test", Email.From("test@example.com"), "password", UserRole.Admin));

        var command = Command with { Latitude = double.NaN, Longitude = double.NaN };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(DomainErrors.Location.NullLatitude);
    }

    // add hotel to city and save changes
    [Fact]
    public async Task Handle_WhenHotelIsCreated_ReturnsHotelResponse()
    {
        // Arrange
        _userIdentifierProviderMock.Role.Returns(UserRole.Admin);
        _cityRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), default)
            .Returns(new City(Command.Name, Command.Description, "PS"));
        _userRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), default)
            .Returns(new User("test", Email.From("test@example.com"), "password", UserRole.Admin));

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert

        result.IsSuccess.Should().BeTrue();
    }
}
