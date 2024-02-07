using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Abstractions.Sessions;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Bookings;
using Tap.Domain.Features.Photos;
using Tap.Infrastructure.Payment.Options;
using Session = Tap.Domain.Common.Session;

namespace Tap.Infrastructure.Payment;

public class StripeService : ISessionService
{
    private readonly IDbContext _dbContext;
    private readonly StripeOptions _stripeOptions;
    private readonly SessionService _sessionService;

    private const string DefaultImageUrl =
        "https://i.imgur.com/iw0UgTm_d.webp?maxwidth=1520&fidelity=grand";

    public StripeService(
        IDbContext dbContext,
        IOptions<StripeOptions> stripeOptions,
        SessionService sessionService
    )
    {
        _dbContext = dbContext;
        _sessionService = sessionService;
        _stripeOptions = stripeOptions.Value;
    }

    public async Task<string> GetByIdAsync(
        string sessionId,
        CancellationToken cancellation = default
    )
    {
        var session = await _sessionService.GetAsync(sessionId, cancellationToken: cancellation);

        return session.Id;
    }

    public async Task<Maybe<Session>> SaveAsync(int bookingId, CancellationToken cancellation)
    {
        var booking = await _dbContext
            .Set<Booking>()
            .Include(b => b.Room)
            .Include(b => b.Hotel)
            .ThenInclude(h => h.City)
            .FirstOrDefaultAsync(b => b.Id == bookingId, cancellation);

        if (booking is null)
        {
            return Maybe<Session>.None;
        }

        var photos = await _dbContext
            .Set<Photo>()
            .Where(p => p.ItemId == booking.RoomId)
            .Select(p => p.Url)
            .ToListAsync(cancellation);

        var images = photos.Any() ? photos : new List<string> { DefaultImageUrl };

        var options = new SessionCreateOptions
        {
            SuccessUrl = _stripeOptions.SuccessUrl,
            CancelUrl = _stripeOptions.CancelUrl,
            PaymentMethodTypes = _stripeOptions.PaymentMethods,
            Mode = _stripeOptions.Mode,
            LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)booking.TotalPrice * 100,
                        Currency = booking.Room.Price.Currency,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"{booking.Room.Type} Room - {booking.Room.Number}",
                            Description = $"""
                                  Secure your stay in the exquisite {booking.Room.Type} Room at {booking.Hotel.Name},
                                  nestled in the heart of {booking.Hotel.City.Name}. Your booking awaits in Room
                                  {booking.Room.Number}—indulge in unparalleled comfort and style.
                                  """,
                            Images = images
                        }
                    },
                    Quantity = 1
                }
            }
        };

        var session = await _sessionService.CreateAsync(options, cancellationToken: cancellation);

        return Maybe<Session>.From(new Session(session.Id, _stripeOptions.PublishableKey));
    }
}
