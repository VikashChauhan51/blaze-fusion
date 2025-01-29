namespace BlazeFusion.Sample.Pages.Components;

public class Counter : BlazeComponent
{
    public int Count { get; set; }
    public void Add()
    {
        Count++;
    }
}
