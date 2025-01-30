namespace BlazeFusion;

/// <summary>
/// Represents a subscription to a Blaze event.
/// </summary>
internal sealed class BlazeEventSubscription
{
    /// <summary>
    /// Gets or sets the name of the event.
    /// </summary>
    public string EventName { get; set; }

    /// <summary>
    /// Gets or sets the function that retrieves the subject of the event.
    /// </summary>
    public Func<string> SubjectRetriever { get; set; }

    /// <summary>
    /// Gets or sets the action to be performed when the event is triggered.
    /// </summary>
    public Delegate Action { get; set; }
}
