using Microsoft.AspNetCore.Http;


namespace BlazeFusion;

/// <summary>
/// Component result
/// </summary>
public interface IComponentResult
{
    /// <summary>
    /// Execute the result
    /// </summary>
    Task ExecuteAsync(HttpContext httpContext, BlazeComponent component);
}
