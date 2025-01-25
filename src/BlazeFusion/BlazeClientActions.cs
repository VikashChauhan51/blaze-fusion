namespace BlazeFusion;

/// <summary>
/// Actions that are evaluated on the client side
/// </summary>
public class BlazeClientActions
{
    private readonly BlazeComponent _blazeComponent;

    internal BlazeClientActions(BlazeComponent blazeComponent) =>
        _blazeComponent = blazeComponent;

    /// <summary>
    /// Execute JavaScript expression on client side
    /// </summary>
    /// <param name="jsExpression">JavaScript expression</param>
    public void ExecuteJs(string jsExpression) =>
        _blazeComponent.AddClientScript(jsExpression);

    /// <summary>
    /// Dispatch a Blaze event
    /// </summary>
    public void Dispatch<TEvent>(TEvent data, Scope scope, bool asynchronous) =>
        _blazeComponent.Dispatch(data, scope, asynchronous);

    /// <summary>
    /// Dispatch a Blaze event
    /// </summary>
    public void Dispatch<TEvent>(TEvent data, Scope scope) =>
        _blazeComponent.Dispatch(data, scope);

    /// <summary>
    /// Dispatch a Blaze event
    /// </summary>
    public void Dispatch<TEvent>(TEvent data) =>
        _blazeComponent.Dispatch(data);

    /// <summary>
    /// Dispatch a Blaze event
    /// </summary>
    public void DispatchGlobal<TEvent>(TEvent data) =>
        _blazeComponent.DispatchGlobal(data);

    /// <summary>
    /// Dispatch a Blaze event
    /// </summary>
    public void DispatchGlobal<TEvent>(TEvent data, string subject) =>
        _blazeComponent.DispatchGlobal(data, subject: subject);
}
