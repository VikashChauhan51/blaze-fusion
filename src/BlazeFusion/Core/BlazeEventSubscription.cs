namespace BlazeFusion;
internal class BlazeEventSubscription
{
    public string EventName { get; set; }
    public Func<string> SubjectRetriever { get; set; }
    public Delegate Action { get; set; }
}
