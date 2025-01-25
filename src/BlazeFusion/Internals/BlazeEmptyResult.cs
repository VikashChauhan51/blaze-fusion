using Microsoft.AspNetCore.Http;

namespace BlazeFusion;

internal sealed class BlazeEmptyResult : IResult
{
    private BlazeEmptyResult()
    {
    }

    public static BlazeEmptyResult Instance { get; } = new();
    public Task ExecuteAsync(HttpContext httpContext) => Task.CompletedTask;
}
