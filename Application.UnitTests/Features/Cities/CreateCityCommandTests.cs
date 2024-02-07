using FluentAssertions;
using NSubstitute;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Features.Authentication;
using Tap.Application.Features.Cities.CreateCity;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Cities;
using Tap.Domain.Features.Users;

namespace Application.UnitTests.Features.Cities;

public class CreateCityCommandTests
{
    private readonly ICityRepository _cityRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly IUserContext _userContextMock;

    private static readonly CreateCityCommand Command = new("City", "Description", "Country");

    private readonly CreateCityCommandHandler _sut;

    public CreateCityCommandTests()
    {
        _cityRepositoryMock = Substitute.For<ICityRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _userContextMock = Substitute.For<IUserContext>();

        _sut = new CreateCityCommandHandler(
            _cityRepositoryMock,
            _unitOfWorkMock,
            _userContextMock
        );
    }

    [Fact]
    public async Task Handle_Should_ReturnError_When_UserIsNotAdmin()
    {
        // Arrange
        _userContextMock.Role.Returns(UserRole.User);

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(DomainErrors.User.Unauthorized);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_When_CityAlreadyExists()
    {
        // Arrange
        _userContextMock.Role.Returns(UserRole.Admin);
        _cityRepositoryMock
            .GetByNameAsync(Command.Name, CancellationToken.None)
            .Returns(new City(Command.Name, Command.Description, Command.Country));

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(DomainErrors.City.AlreadyExists);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_When_CityDoesNotExist()
    {
        // Arrange
        _userContextMock.Role.Returns(UserRole.Admin);
        _cityRepositoryMock
            .GetByNameAsync(Command.Name, CancellationToken.None)
            .Returns(Maybe<City>.None);
        _cityRepositoryMock.Insert(Arg.Any<City>());

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_CallUnitOfWork_When_CityDoesNotExist()
    {
        // Arrange
        _userContextMock.Role.Returns(UserRole.Admin);
        _cityRepositoryMock
            .GetByNameAsync(Command.Name, CancellationToken.None)
            .Returns(Maybe<City>.None);
        _cityRepositoryMock.Insert(Arg.Any<City>());

        // Act
        await _sut.Handle(Command, CancellationToken.None);

        // Assert
        await _unitOfWorkMock.Received(1).SaveChangesAsync(CancellationToken.None);
    }
}
