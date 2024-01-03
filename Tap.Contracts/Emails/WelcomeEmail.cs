namespace Tap.Contracts.Emails;

public record WelcomeEmail(string Name, string EmailTo, string Token);

public record BookingSuccessEmail(
    string Name,
    string EmailTo,
    string HotelName,
    string CheckInDate,
    string CheckOutDate,
    decimal TotalPrice,
    string Currency
);
