using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace BlazeFusion;

/// <summary>  
/// Represents a dummy view that does nothing.  
/// </summary>  
internal sealed class DummyView : IView
{
    /// <summary>  
    /// Gets the path of the view.  
    /// </summary>  
    public string Path => string.Empty;

    /// <summary>  
    /// Renders the view asynchronously.  
    /// </summary>  
    /// <param name="context">The view context.</param>  
    /// <returns>A completed task.</returns>  
    public Task RenderAsync(ViewContext context) => Task.CompletedTask;
}

