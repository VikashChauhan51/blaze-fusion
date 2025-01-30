using Microsoft.AspNetCore.Html;

namespace BlazeFusion;

/// <summary>
/// Represents a context for managing HTML slot items.
/// </summary>
internal sealed class SlotContext
{
    /// <summary>
    /// Gets or sets the dictionary of slot items, where the key is the slot name and the value is the HTML content.
    /// </summary>
    public Dictionary<string, HtmlString> Items { get; set; } = new();
}
