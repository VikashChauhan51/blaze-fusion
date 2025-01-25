using Microsoft.AspNetCore.Http;

namespace BlazeFusion;
 
/// <summary>
/// A filter that confirms component authorization
/// </summary>
public interface IBlazeAuthorizationFilter
{
    /// <summary>
    /// Called early in the component pipeline to confirm request is authorized
    /// </summary>
    /// <param name="httpContext">HttpContext</param>
    /// <param name="component">Blaze component instance</param>
    /// <returns>Indication if the the operation is authorized</returns>
    Task<bool> AuthorizeAsync(HttpContext httpContext, object component);
}
