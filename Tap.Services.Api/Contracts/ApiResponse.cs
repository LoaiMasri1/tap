using System.Net;
using Tap.Domain.Core.Primitives;

namespace Tap.Services.Api.Contracts;

public class ApiResponse<TValue, TError>
{
    public TValue? Data { get; set; } = default;
    public IReadOnlyCollection<TError>? Errors { get; set; } = default;
    public string? Message { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}

public class ApiResponse : ApiResponse<object, Error> { }
