using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace BlazeFusion;
internal class DummyView : IView
{
    public string Path => string.Empty;
    public Task RenderAsync(ViewContext context) => Task.CompletedTask;
}

