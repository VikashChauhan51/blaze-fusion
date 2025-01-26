# blaze-fusion

[![NuGet Version](https://img.shields.io/nuget/v/BlazeFusion.svg?style=flat-square)](https://www.nuget.org/packages/BlazeFusion/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/BlazeFusion.svg?style=flat-square)](https://www.nuget.org/packages/BlazeFusion/)
[![Build Status](https://github.com/VikashChauhan51/blaze-fusion/actions/workflows/build.yml/badge.svg)](https://github.com/VikashChauhan51/blaze-fusion/actions)
[![License](https://img.shields.io/github/license/VikashChauhan51/blaze-fusion.svg?style=flat-square)](https://github.com/VikashChauhan51/blaze-fusion/blob/main/LICENSE)


BlazeFusion is an extension to ASP.NET Core MVC and Razor Pages. It extends View Components to make them reactive and stateful with ability to communicate with each other without page reloads. As a result, you can create powerful components and make your application to feel like SPA with zero or minimal amount of the JavaScript code (depending on the needs) and without separate front-end build step. It can be used in new or existing ASP.NET Core applications.

BlazeFusion utilizes the following technologies to make it all work:

- **Razor views (\*.cshtml)**
Razor views form the backbone of BlazeFusion's UI generation. They allow for a familiar, server-side rendering strategy that has been a foundation of .NET web development for many years. These *.cshtml files enable a seamless mix of HTML and C# code, allowing for robust and dynamic webpage generation.


- **AJAX**
AJAX calls are used to communicate between the client and the server, specifically to send the application state to the server, receive updates and responses back, and then store this state to be used in subsequent requests. This ensures that each request has the most up-to-date context and information.


- **Alpine.js**
Alpine.js stands as a base for requests execution and  DOM swapping. But beyond that, Alpine.js also empowers users by providing a framework for adding rich, client-side interactivity to the standard HTML. So, not only does it serve BlazeFusion's internal operations, but it also provides an expansion point for users to enhance their web applications with powerful, interactive experiences.


## Installation

In ASP.NET Core Razor Pages / MVC project 6.0+ install BlazeFusion package:
```console
dotnet add package BlazeFusion
```

If you don't have application yet, you can create it first:

```console
dotnet new webapp -o MyApp
cd MyApp
```
In your MyApp.csproj file make sure ou have added a Root namespace:

```
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
   <RootNamespace>MyApp</RootNamespace>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="BlazeFusion" Version="0.0.0.2" />
  </ItemGroup>

</Project>
```

In your application's startup code (either `Program.cs` or `Startup.cs`):

```c#
builder.Services.AddBlaze();

...
app.UseStaticFiles();
app.UseBlaze(builder.Environment);
```

In `_ViewImports.cshtml` add:
```razor
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
```c#
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
## External libraries

- [Alpine.js](https://github.com/alpinejs/alpine) libraries MIT licensed.
- [HtmlAgilityPack ](https://github.com/zzzprojects/html-agility-pack/) libraries MIT licensed.



