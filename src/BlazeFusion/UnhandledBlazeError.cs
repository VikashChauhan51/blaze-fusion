namespace BlazeFusion;

/// <summary>
/// Event called in case of unhandled exception in Blaze component
/// </summary>
/// <param name="Message">Exception message</param>
/// <param name="Data">Payload</param>
public record UnhandledBlazeError(string Message, object Data);
