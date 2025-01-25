using Microsoft.AspNetCore.Html;

namespace BlazeFusion;
internal class SlotContext
{
    public Dictionary<string, HtmlString> Items { get; set; } = new();
}
