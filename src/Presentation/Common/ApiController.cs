using Microsoft.AspNetCore.Mvc;

namespace Presentation.Common;

/// <summary>
/// Base class for all API controllers. has some boilerplate setup.
/// </summary>
[ApiController]
[Produces("application/json")]
[Route("/api/[controller]")]
public abstract class ApiController : ControllerBase
{
    /// <summary>
    /// Gets the required service from the request services.
    /// </summary>
    protected T GetRequiredService<T>() where T : notnull => HttpContext.RequestServices.GetRequiredService<T>();
}