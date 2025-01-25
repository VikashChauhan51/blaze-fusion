using Microsoft.AspNetCore.Http;

namespace BlazeFusion;

/// <summary>
/// Blaze extensions for HttpContext
/// </summary>
public static class BlazeHttpContextExtensions
{
    /// <summary>
    /// Indicates if the request is going through Blaze's pipeline
    /// </summary>
    /// <param name="httpContext">HttpContext instance</param>
    /// <param name="excludeBoosted">Return false for boosted requests</param>
    public static bool IsBlaze(this HttpContext httpContext, bool excludeBoosted = false) =>
        httpContext.Request.Headers.ContainsKey(BlazeConsts.RequestHeaders.Blaze)
        && (!excludeBoosted || !httpContext.IsBlazeBoosted());

    /// <summary>
    /// Indicates if the request is using Blaze's boost functionality
    /// </summary>
    /// <param name="httpContext">HttpContext instance</param>
    public static bool IsBlazeBoosted(this HttpContext httpContext) =>
        httpContext.Request.Headers.ContainsKey(BlazeConsts.RequestHeaders.Boosted);
}
