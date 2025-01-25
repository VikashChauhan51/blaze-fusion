using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace BlazeFusion;



internal class ComponentResult : IComponentResult
{
    private readonly IResult _result;
    private readonly ComponentResultType _type;

    internal ComponentResult(IResult result, ComponentResultType type)
    {
        _result = result;
        _type = type;
    }

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
            //component.Redirect(location); //TODO: pending
        }
    }
}
