namespace Tap.Contracts.Emails;

public record BookingPayedEmail(
    int BookingId,
    string EmailTo,
    string Name,
    string HotelName,
    decimal TotalPrice,
    string Currency
);
