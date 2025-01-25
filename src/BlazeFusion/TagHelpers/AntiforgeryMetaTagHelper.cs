using Microsoft.AspNetCore.Antiforgery;
using System.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using BlazeFusion.Configuration;
using System.Text.Json;

namespace BlazeFusion.TagHelpers;
 
/// <summary>
/// Provides Blaze options serialized to a meta tag
/// </summary>
[HtmlTargetElement("meta", Attributes = "[name=blaze-config]")]
public sealed class BlazeConfigMetaTagHelper : TagHelper
{
    /// <summary>
    /// View context
    /// </summary>
    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; }

    /// <summary>
    /// Processes the output
    /// </summary>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var BlazeOptions = ViewContext.HttpContext.RequestServices.GetService<BlazeOptions>();

        var config = JsonSerializer.Serialize(GetConfig(BlazeOptions));

        output.Attributes.RemoveAll("content");

        output.Attributes.Add(new TagHelperAttribute(
            "content",
            new HtmlString(config),
            HtmlAttributeValueStyle.SingleQuotes)
        );
    }

    private object GetConfig(BlazeOptions options) => new
    {
        Antiforgery = GetAntiforgeryConfig(options)
    };

    private AntiforgeryConfig GetAntiforgeryConfig(BlazeOptions options)
    {
        if (!options.AntiforgeryTokenEnabled)
        {
            return null;
        }

        var antiforgery = ViewContext.HttpContext.RequestServices.GetService<IAntiforgery>();

        return antiforgery?.GetAndStoreTokens(ViewContext.HttpContext) is { } tokens
            ? new AntiforgeryConfig(tokens)
            : null;
    }

    private class AntiforgeryConfig
    {
        public AntiforgeryConfig(AntiforgeryTokenSet antiforgery)
        {
            ArgumentNullException.ThrowIfNull(antiforgery);

            HeaderName = antiforgery.HeaderName;
            Token = HttpUtility.HtmlAttributeEncode(antiforgery.RequestToken)!;
        }

        public string HeaderName { get; set; }
        public string Token { get; set; }
    }
}
