namespace Tap.Contracts.Emails;

public record BookingSuccessEmail(
    string Name,
    string EmailTo,
    string HotelName,
    string CheckInDate,
    string CheckOutDate,
    decimal TotalPrice,
    string Currency
);
