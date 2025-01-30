namespace BlazeFusion;

/// <summary>
/// Represents an event within a Blaze component.
/// </summary>
internal sealed class BlazeComponentEvent
{
    /// <summary>
    /// Gets the name of the event.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Gets the subject of the event.
    /// </summary>
    public string Subject { get; init; }

    /// <summary>
    /// Gets the data associated with the event.
    /// </summary>
    public object Data { get; init; }

    /// <summary>
    /// Gets or sets the scope of the event.
    /// </summary>
    public string Scope { get; set; }

    /// <summary>
    /// Gets or sets the operation ID associated with the event.
    /// </summary>
    public string OperationId { get; set; }
}
