using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Application.Features.Authentication;
using Tap.Contracts.Features.Reviews;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Hotels;
using Tap.Domain.Features.Reviews;
using Tap.Domain.Features.Users;

namespace Tap.Application.Features.Reviews.CreateReview;

public class CreateReviewCommandHandler
    : ICommandHandler<CreateReviewCommand, Result<ReviewResponse>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public CreateReviewCommandHandler(
        IHotelRepository hotelRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    )
    {
        _hotelRepository = hotelRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result<ReviewResponse>> Handle(
        CreateReviewCommand request,
        CancellationToken cancellationToken
    )
    {
        if (_userContext.Role == UserRole.Admin)
        {
            return DomainErrors.User.Unauthorized;
        }

        var userId = _userContext.Id;

        if (userId != request.UserId)
        {
            return DomainErrors.User.Unauthorized;
        }

        var maybeHotel = await _hotelRepository.GetHotelIncludeRatingsAsync(
            request.HotelId,
            cancellationToken
        );
        if (maybeHotel.HasNoValue)
        {
            return DomainErrors.Hotel.NotFound;
        }

        var maybeUser = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (maybeUser.HasNoValue)
        {
            return DomainErrors.User.NotFound;
        }

        var review = Review.Create(
            request.Title,
            request.Content,
            request.Rating,
            maybeHotel.Value,
            maybeUser.Value
        );

        var hotel = maybeHotel.Value;

        hotel.AddReview(review);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ReviewResponse(
            review.Id,
            review.Title,
            review.Content,
            review.Rating,
            review.HotelId,
            review.UserId
        );
    }
}
