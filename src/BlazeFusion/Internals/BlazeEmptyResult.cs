using Microsoft.AspNetCore.Http;

namespace BlazeFusion;

/// <summary>
/// Represents an empty result that does nothing when executed.
/// </summary>
internal sealed class BlazeEmptyResult : IResult
{
    /// <summary>
    /// Prevents a default instance of the <see cref="BlazeEmptyResult"/> class from being created.
    /// </summary>
    private BlazeEmptyResult()
    {
    }

    /// <summary>
    /// Gets the singleton instance of the <see cref="BlazeEmptyResult"/> class.
    /// </summary>
    public static BlazeEmptyResult Instance { get; } = new();

    /// <summary>
    /// Executes the result operation of the action method asynchronously.
    /// </summary>
    /// <param name="httpContext">The <see cref="HttpContext"/> for the current request.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public Task ExecuteAsync(HttpContext httpContext) => Task.CompletedTask;
}
