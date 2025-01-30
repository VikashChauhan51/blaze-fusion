namespace BlazeFusion;

/// <summary>
/// Represents a polling action with a specified interval.
/// </summary>
/// <param name="Action">The action to be performed during polling.</param>
/// <param name="Interval">The interval between each polling action.</param>
internal sealed record BlazePoll(string Action, TimeSpan Interval);
