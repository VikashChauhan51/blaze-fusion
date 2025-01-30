---
outline: deep
---

# Errors handling

In regular cases, expected errors handling can be done manually by `try/catch` statements.
But in cases of unhandled exceptions we want to gracefully inform the user of the situation.

One of the ways to do it when working with BlazeFusion components is to create a global messages
component for showing alerts, that can be rendered in your layout. Such component could
subscribe to your own custom events with messages, but it also can listen to built-in
BlazeFusion's unhandled error event: `UnhandledBlazeError`.

```c#
public class Toasts : BlazeComponent
{
    public List<Toast> ToastsList { get; set; } = new();

    public Toasts()
    {
        Subscribe<UnhandledBlazeError>(Handle);
    }

    private void Handle(UnhandledBlazeError data) =>
        ToastsList.Add(new Toast(
            Id: Guid.NewGuid().ToString("N"),
            Message: data.Message,
            Type: ToastType.Error
        ));

    public void Close(string id) =>
        ToastsList.RemoveAll(t => t.Id == id);

    public record Toast(string Id, string Message, ToastType Type);
}
```

BlazeFusion will send `UnhandledBlazeError` in case of unhandled error and by default
it will contain the response from the server produced by ASP.NET MVC for exceptions,
which might be to expressive. To customize that you can configure the ASP.NET MVC exception
handling:

```c#
app.UseExceptionHandler(b => b.Run(async context =>
{
    if (!context.IsBlaze())
    {
        context.Response.Redirect("/Error");
        return;
    }

    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
    switch (contextFeature?.Error)
    {
        // custom cases for custom exception types if needed

        default:
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsJsonAsync(new UnhandledBlazeError(
                Message: "There was a problem with this operation and it wasn't finished",
                Data: null
            ));

            return;
    }
}));
```

In the code above we are creating a JSON response containing `UnhandledBlazeError` event that
will be consumed in our `Toasts` component.
