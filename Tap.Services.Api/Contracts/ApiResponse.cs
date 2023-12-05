using System.Net;

namespace Tap.Services.Api.Contracts;

public class ApiResponse<TValue, TError>
{
    public TValue? Data { get; set; } = default;
    public TError? Errors { get; set; } = default;
    public string? Message { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}
