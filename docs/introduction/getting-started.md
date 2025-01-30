---
outline: deep
---

# Getting Started

Create your web application with the following command. If you already have an app using ASP.NET Core 6+ Razor Pages / MVC project, you can skip this step.

## Configuration

```console
dotnet new webapp -o MyApp
cd MyApp
```
Install BlazeFusion [NuGet package](https://www.nuget.org/packages/BlazeFusion/):

```console
dotnet add package BlazeFusion
```
In application's startup code (either `Program.cs` or `Startup.cs`) add:

```csharp
builder.Services.AddBlaze();

...
app.UseStaticFiles(); // ensure use static files middleware is configured
app.UseBlaze(builder.Environment);
```
> **NOTE:** Make sure that UseBlazeis called after `UseStaticFiles` and `UseRouting`, which are required.

Sample `Program.cs` file:

```csharp
using BlazeFusion;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddBlaze();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseRouting();
app.UseStaticFiles();
app.MapRazorPages();
app.UseBlaze(builder.Environment);
app.Run();
```
In `_ViewImports.cshtml` add:

```console
@addTagHelper *, {Your project assembly name}
@addTagHelper *, BlazeFusion
```
In layout's `head` tag:
```html
<meta name="blaze-config" />
<script defer src="~/blaze/blaze.js" asp-append-version="true"></script>
<script defer src="~/blaze/alpine.js" asp-append-version="true"></script>
```

## Quick start
To create BlazeFusion component, go to your components folder, for example in case of Razor Pages: `~/Pages/Components/`, and create these files:

```razor
<!-- Counter.cshtml -->

@model Counter

<div>
  Count: <strong>@Model.Count</strong>
  <button on:click="@(() => Model.Add())">
    Add
  </button>
</div>
```
```csharp
// Counter.cs

public class Counter : BlazeComponent
{
    public int Count { get; set; }

    public void Add()
    {
        Count++;
    }
}
```

### Usage

To use your new component, you can render it in your Razor Page (e.g. `Index.cshtml`) in two ways:

by calling a custom tag:
```razor
...
<counter />
...
```

by calling a generic tag helper:

```razor
...
<Blaze name="Counter"/>
...
```

or by calling an extension method:
```razor
...
@await Html.Blaze("Counter")
...
```
