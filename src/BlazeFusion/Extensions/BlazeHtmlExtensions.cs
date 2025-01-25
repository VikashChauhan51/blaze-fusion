using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlazeFusion;

/// <summary>
///  Blaze html tag helper
/// </summary>
public static class BlazeHtmlExtensions
{
    /// <summary>
    /// Renders Blaze component
    /// </summary>
    public static async Task<IHtmlContent> Blaze<TComponent>(this IHtmlHelper helper, object parameters = null, string key = null) where TComponent : BlazeComponent
    {
        var arguments = parameters != null || key != null
            ? new { Params = parameters, Key = key }
            : null;

        return await TagHelperRenderer.RenderTagHelper(
            componentType: typeof(TComponent),
            httpContext: helper.ViewContext.HttpContext,
            parameters: PropertyExtractor.GetPropertiesFromObject(arguments)
        );
    }

    /// <summary>
    /// Renders Blaze component
    /// </summary>
    public static async Task<IHtmlContent> Blaze(this IHtmlHelper helper, string BlazeComponentName, object parameters = null, string key = null)
    {
        var tagHelper = TagHelperRenderer.FindTagHelperType(BlazeComponentName, helper.ViewContext.HttpContext);

        var arguments = parameters != null || key != null
            ? new { Params = parameters, Key = key }
            : null;

        return await TagHelperRenderer.RenderTagHelper(
            componentType: tagHelper,
            httpContext: helper.ViewContext.HttpContext,
            parameters: PropertyExtractor.GetPropertiesFromObject(arguments)
        );
    }
}
