using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BlazeFusion.TagHelpers;
 
/// <summary>
/// Provides a mechanism to load the target url in the background and witch the body content when ready
/// </summary>
[HtmlTargetElement("*", Attributes = TagAttribute)]
public sealed class BlazeLinkTagHelper : TagHelper
{
    private const string TagAttribute = "blaze-link";

    /// <summary>
    /// Attribute that triggers the boost behavior
    /// </summary>
    [HtmlAttributeName(TagAttribute)]
    public bool Link { get; set; } = true;

    /// <summary>
    /// Processes the tag helper
    /// </summary>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(output);

        output.Attributes.RemoveAll("blaze-link");
        output.Attributes.Add(new("x-data"));
        output.Attributes.Add(new("x-blaze-link"));
    }
}
