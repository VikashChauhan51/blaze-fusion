using Microsoft.AspNetCore.Mvc.Razor;

namespace BlazeFusion;
 

/// <summary>
///  Page extension
/// </summary>
public static class BlazePageExtensions
{
    /// <summary>
    /// Specifies selector to use when replacing content of the page
    /// </summary>
    public static void BlazeTarget(this IRazorPage page, string selector = $"#{BlazeComponent.LocationTargetId}", string title = null)
    {
        page.ViewContext.HttpContext.Response.Headers.TryAdd(BlazeConsts.ResponseHeaders.LocationTarget, selector);

        if (!string.IsNullOrEmpty(title))
        {
            page.ViewContext.HttpContext.Response.Headers.TryAdd(BlazeConsts.ResponseHeaders.LocationTitle, title);
        }
    }
}
