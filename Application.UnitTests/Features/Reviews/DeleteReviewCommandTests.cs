using FluentAssertions;
using NSubstitute;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Features.Authentication;
using Tap.Application.Features.Reviews.DeleteReview;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Hotels;
using Tap.Domain.Features.Reviews;
using Tap.Domain.Features.Users;

namespace Application.UnitTests.Features.Reviews;

public class DeleteReviewCommandTests
{
    private readonly IReviewRepository _reviewRepositoryMock;
    private readonly IUserContext _userContextMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    private static readonly DeleteReviewCommand Command = new(1);

    private static readonly User User =
        new("Name", Email.From("test@example.com"), "Password", UserRole.Admin);

    private static readonly Hotel Hotel =
        new("Name", "Description", Location.Create(35.4, 35.5).Value, User);

    private static readonly Review Review = Review.Create("Title", "Body", 5, Hotel, User);

    private readonly DeleteReviewCommandHandler _sut;

    public DeleteReviewCommandTests()
    {
        _reviewRepositoryMock = Substitute.For<IReviewRepository>();
        _userContextMock = Substitute.For<IUserContext>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _sut = new DeleteReviewCommandHandler(
            _reviewRepositoryMock,
            _userContextMock,
            _unitOfWorkMock
        );

        _userContextMock.Id.Returns(1);
    }

    [Fact]
    public async Task Handle_WhenUserIsNotOwner_ReturnsUnauthorized()
    {
        _reviewRepositoryMock
            .GetByIdAsync(Command.Id, Arg.Any<CancellationToken>())
            .Returns(Maybe<Review>.From(Review));

        _userContextMock.Id.Returns(2);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.User.Unauthorized);
    }

    [Fact]
    public async Task Handle_WhenUserIsNotAdmin_ReturnsUnauthorized()
    {
        _reviewRepositoryMock
            .GetByIdAsync(Command.Id, Arg.Any<CancellationToken>())
            .Returns(Maybe<Review>.From(Review));

        _userContextMock.Role.Returns(UserRole.User);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.User.Unauthorized);
    }

    [Fact]
    public async Task Handle_WhenReviewNotFound_ReturnsNotFound()
    {
        _reviewRepositoryMock
            .GetByIdAsync(Command.Id, Arg.Any<CancellationToken>())
            .Returns(Maybe<Review>.None);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Review.NotFound);
    }

    [Fact]
    public async Task Handle_WhenReviewFound_DeletesReview()
    {
        _userContextMock.Id.Returns(User.Id);
        _userContextMock.Role.Returns(UserRole.Admin);

        _reviewRepositoryMock
            .GetByIdAsync(Command.Id, Arg.Any<CancellationToken>())
            .Returns(Maybe<Review>.From(Review));

        var result = await _sut.Handle(Command, CancellationToken.None);

        _reviewRepositoryMock.Received().Remove(Review);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenReviewFound_SavesChanges()
    {
        _userContextMock.Id.Returns(User.Id);
        _userContextMock.Role.Returns(UserRole.Admin);

        _reviewRepositoryMock
            .GetByIdAsync(Command.Id, Arg.Any<CancellationToken>())
            .Returns(Maybe<Review>.From(Review));

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        await _unitOfWorkMock.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
