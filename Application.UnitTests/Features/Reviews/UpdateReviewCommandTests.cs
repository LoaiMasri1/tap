using FluentAssertions;
using NSubstitute;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Features.Authentication;
using Tap.Application.Features.Reviews.UpdateReview;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Hotels;
using Tap.Domain.Features.Reviews;
using Tap.Domain.Features.Users;

namespace Application.UnitTests.Features.Reviews;

public class UpdateReviewCommandTests
{
    private readonly IReviewRepository _reviewRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly IUserContext _userContextMock;

    private static readonly UpdateReviewCommand Command = new(1, "Title", "Content", 5);
    private static readonly User User =
        new("Name", Email.From("test@example.com"), "Password", UserRole.Admin);

    private static readonly Hotel Hotel =
        new("Name", "Description", Location.Create(35.4, 35.5).Value, User);

    private static readonly Review Review = Review.Create("Title", "Body", 5, Hotel, User);

    private readonly UpdateReviewCommandHandler _sut;

    public UpdateReviewCommandTests()
    {
        _reviewRepositoryMock = Substitute.For<IReviewRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _userContextMock = Substitute.For<IUserContext>();

        _sut = new UpdateReviewCommandHandler(
            _reviewRepositoryMock,
            _unitOfWorkMock,
            _userContextMock
        );
    }

    [Fact]
    public async Task Handle_WhenUserUnauthorized_ReturnsUnauthorized()
    {
        _reviewRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Review);

        _userContextMock.Id.Returns(2);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.User.Unauthorized);
    }

    [Fact]
    public async Task Handle_WhenUserIsAdmin_ReturnsUnauthorized()
    {
        _userContextMock.Role.Returns(UserRole.Admin);

        _reviewRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Review);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.Error.Should().Be(DomainErrors.User.Unauthorized);
    }

    [Fact]
    public async Task Handle_WhenReviewNotFound_ReturnsNotFound()
    {
        _reviewRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Maybe<Review>.None);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Review.NotFound);
    }

    [Fact]
    public async Task Handle_WhenReviewFound_UpdatesReview()
    {
        _userContextMock.Id.Returns(User.Id);
        _userContextMock.Role.Returns(UserRole.User);

        _reviewRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Review);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Title.Should().Be(Command.Title);
        result.Value.Content.Should().Be(Command.Content);
        result.Value.Rating.Should().Be(Command.Rating);
    }

    [Fact]
    public async Task Handle_WhenReviewFoundAndSame_ReturnsNothingToUpdate()
    {
        _userContextMock.Id.Returns(User.Id);
        _userContextMock.Role.Returns(UserRole.User);

        _reviewRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Review);

        Review.Update(Command.Title, Command.Content, Command.Rating);

        var result = await _sut.Handle(
            Command with
            {
                Title = Review.Title
            },
            CancellationToken.None
        );

        result.Error.Should().Be(DomainErrors.Review.NothingToUpdate);
    }

    [Fact]
    public async Task Handle_WhenReviewFound_SavesChanges()
    {
        _userContextMock.Id.Returns(User.Id);
        _userContextMock.Role.Returns(UserRole.User);

        _reviewRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Review);

        Review.Update("New Title", "New Content", 5);

        var result = await _sut.Handle(Command, CancellationToken.None);

        await _unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());

        result.IsSuccess.Should().BeTrue();
    }
}
