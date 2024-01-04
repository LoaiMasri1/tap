namespace Tap.Contracts.Emails;

public record BookingConfirmedEmail(
    int BookingId,
    string Name,
    string EmailTo,
    string HotelName,
    string CheckInDate,
    string CheckOutDate,
    decimal TotalPrice,
    string Currency
);

public record BookingCanceledEmail(string Name, string EmailTo, string HotelName);
