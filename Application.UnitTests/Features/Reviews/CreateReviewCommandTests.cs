using FluentAssertions;
using NSubstitute;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Features.Authentication;
using Tap.Application.Features.Reviews.CreateReview;
using Tap.Contracts.Features.Reviews;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Hotels;
using Tap.Domain.Features.Users;

namespace Application.UnitTests.Features.Reviews;

public class CreateReviewCommandTests
{
    private readonly IHotelRepository _hotelRepositoryMock;
    private readonly IUserRepository _userRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly IUserContext _userContextMock;

    private static readonly CreateReviewCommand Command = new("Title", "Content", 5, 1, 1);

    private static readonly User User =
        new("Name", Email.From("test@example.com"), "Password", UserRole.Admin);

    private static readonly Hotel Hotel =
        new("Name", "Description", Location.Create(35.4, 35.5).Value, User);

    private readonly CreateReviewCommandHandler _sut;

    public CreateReviewCommandTests()
    {
        _hotelRepositoryMock = Substitute.For<IHotelRepository>();
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _userContextMock = Substitute.For<IUserContext>();
        _sut = new CreateReviewCommandHandler(
            _hotelRepositoryMock,
            _userRepositoryMock,
            _unitOfWorkMock,
            _userContextMock
        );

        _userRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(User);
    }

    [Fact]
    public async Task Handle_WhenUserIsAdmin_ReturnsUnauthorized()
    {
        // Arrange
        _userContextMock.Role.Returns(UserRole.User);

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(DomainErrors.User.Unauthorized);
    }

    [Fact]
    public async Task Handle_WhenHotelNotFound_ReturnsNotFound()
    {
        // Arrange
        _userContextMock.Role.Returns(UserRole.Admin);
        _hotelRepositoryMock
            .GetHotelIncludeRatingsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Maybe<Hotel>.None);

        _userContextMock.Id.Returns(1);

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(DomainErrors.Hotel.NotFound);
    }

    [Fact]
    public async Task Handle_WhenUserNotFound_ReturnsNotFound()
    {
        // Arrange
        _userContextMock.Role.Returns(UserRole.Admin);
        _userContextMock.Id.Returns(1);

        _hotelRepositoryMock
            .GetHotelIncludeRatingsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Hotel);

        _userRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Maybe<User>.None);
        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(DomainErrors.User.NotFound);
    }

    [Fact]
    public async Task Handle_WhenUserIsNotAuthorized_ReturnsUnauthorized()
    {
        // Arrange
        _userContextMock.Role.Returns(UserRole.Admin);

        _hotelRepositoryMock
            .GetHotelIncludeRatingsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Hotel);

        _userRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(User);
        _userContextMock.Id.Returns(2);

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(DomainErrors.User.Unauthorized);
    }

    [Fact]
    public async Task Handle_WhenUserIsAuthorized_ReturnsReview()
    {
        // Arrange
        _userContextMock.Role.Returns(UserRole.Admin);

        _hotelRepositoryMock
            .GetHotelIncludeRatingsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Hotel);

        _userRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(User);
        _userContextMock.Id.Returns(1);

        // Act

        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.Value.Should().BeOfType<ReviewResponse>();
    }

    [Fact]
    public async Task Handle_WhenUserIsAuthorized_SaveChanges()
    {
        // Arrange
        _userContextMock.Role.Returns(UserRole.Admin);

        _hotelRepositoryMock
            .GetHotelIncludeRatingsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Hotel);

        _userRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(User);
        _userContextMock.Id.Returns(1);

        // Act

        await _sut.Handle(Command, CancellationToken.None);

        // Assert
        await _unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
