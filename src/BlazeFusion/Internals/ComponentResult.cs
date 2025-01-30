using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace BlazeFusion;

/// <summary>
/// Represents the result of a component execution.
/// </summary>
internal sealed class ComponentResult : IComponentResult
{
    private readonly IResult _result;
    private readonly ComponentResultType _type;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentResult"/> class.
    /// </summary>
    /// <param name="result">The result to be executed.</param>
    /// <param name="type">The type of the component result.</param>
    internal ComponentResult(IResult result, ComponentResultType type)
    {
        _result = result;
        _type = type;
    }

    /// <summary>
    /// Executes the result operation of the component.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <param name="component">The Blaze component.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task ExecuteAsync(HttpContext httpContext, BlazeComponent component)
    {
        var response = httpContext.Response;

        response.Headers.TryAdd(BlazeConsts.ResponseHeaders.SkipOutput, "True");

        if (_type == ComponentResultType.File)
        {
            response.Headers.Append(HeaderNames.ContentDisposition, "inline");
        }

        try
        {
            await _result.ExecuteAsync(httpContext);
        }
        catch
        {
            response.Headers.Remove(HeaderNames.ContentDisposition);
            throw;
        }

        if (response.Headers.Remove(HeaderNames.Location, out var location))
        {
            response.StatusCode = StatusCodes.Status200OK;
            component.Redirect(location);
        }
    }
}
