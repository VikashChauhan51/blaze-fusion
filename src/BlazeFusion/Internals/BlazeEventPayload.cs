namespace BlazeFusion;
 
/// <summary>
/// Represents the payload of a Blaze event.
/// </summary>
internal sealed class BlazeEventPayload
{
    /// <summary>
    /// Gets or sets the name of the event.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the subject of the event.
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Gets or sets the data associated with the event.
    /// </summary>
    public object Data { get; set; }

    /// <summary>
    /// Gets or sets the scope of the event.
    /// </summary>
    public Scope Scope { get; set; }

    /// <summary>
    /// Gets or sets the operation ID of the event.
    /// </summary>
    public string OperationId { get; set; }
}
