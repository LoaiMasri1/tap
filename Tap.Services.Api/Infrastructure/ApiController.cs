using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Tap.Services.Api.Infrastructure;

[ApiController]
[Produces("application/json")]
[Route("api/v{version:ApiVersion}")]
[ApiVersion("1.0")]
public class ApiController : ControllerBase { }
