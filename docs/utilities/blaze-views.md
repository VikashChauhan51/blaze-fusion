---
outline: deep
---

# BlazeFusion views

BlazeFusion views are an extension built in into BlazeFusion that let you create new kind of view components that can replace partials, editors and regular view components.

Here is a basic example of a BlazeFusion view called `Submit`:

```c#
// Submit.cshtml.cs

public class Submit : BlazeView;
```

```razor
<!-- Submit.cshtml -->

@model Submit

<button class="btn btn-primary" type="submit">
  Save
</button>
```

Now we can use our BlazeFusion view in any other razor view:

```razor
<!-- SomeForm.cshtml -->

<submit />
```

## Naming conventions

There are 2 ways of naming BlazeFusion views:

1. Automatic naming with dash-case notation:

    ```c#
    public class SubmitButton : BlazeView;
    ```

   Usage: `<submit-button />`

2. Manual naming with PascalCase using `nameof`:

    ```c#
    [HtmlTargetElement(nameof(SubmitButton))]
    public class SubmitButton : BlazeView;
    ```

    Usage: `<SubmitButton />`

## Parameters

BlazeFusion views use parameters to pass the data from a caller to the view. Example:

```c#
// Alert.cshtml.cs

public class Alert : BlazeView
{
    public string Message { get; set; }
}
```

```razor
<!-- Alert.cshtml -->

@model Alert

<div class="alert">
  @Model.Message
</div>
```

Now we can set a value on the `message` attribute that will be passed to our `Alert`:

```razor
<!-- Index.cshtml -->

<alert message="Success" />
```

Parameter names are converted to kebab-case when used as attributes on tags, so:
- `Message` property becomes `message` attribute.
- `StatusCode` property becomes `status-code` attribute.

## Passing handlers as parameters

When you need to pass a handler to a BlazeFusion view, you can use the `Expression` type:

```c#
// FormButton.cshtml.cs

public class FormButton : BlazeView
{
    public Expression Click { get; set; }
}
```

```razor
<!-- FormButton.cshtml -->

@model FormButton

<button on:click="@Model.Click">
  @Model.Slot()
</button>
```

Usage:

```razor
<form-button click="@(() => Model.Save())">
  Save
</form-button>
```

## Calling actions of a parent BlazeFusion component

You can call an action of a parent BlazeFusion component from a BlazeFusion view using the `Reference<T>`. Example:

Parent (BlazeFusion component):

```c#
public class Parent : BlazeComponent
{
    public string Value { get; set; }

    public void LoadText(string value)
    {
        Value = value;
    }
}
```

```razor
<!-- Parent.cshtml -->

@model Parent

<child-view />
```

Child (BlazeFusion view):

```c#
public class ChildView : BlazeView;
```

```razor
<!-- ChildView.cshtml -->

@model ChildView

@{
  var parent = Model.Reference<Parent>();
}

<button on:click="@(() => parent.LoadText("Hello!"))">
  Set text
</button>
```

## Dynamic attributes

All attributes passed to the BlazeFusion view by the caller are available in a view definition, even when they are not defined as properties.
We can use this feature to pass optional html attributes to the view, for example:

```c#
// Alert.cshtml.cs

public class Alert : BlazeView
{
    public string Message { get; set; }
}
```

```razor
<!-- Alert.cshtml -->

@model Alert

<div class="alert @Model.Attribute("class")">
  @Model.Message
</div>
```

Now we can set an optional attribute `class` that will be added to the final view:

```razor
<!-- Index.cshtml -->

<alert message="Success" class="alert-green" />
```

## Child content

BlazeFusion views support passing the child html content, so it can be used later when rendering the view. Example:

```c#
// DisplayField.cshtml.cs

public class DisplayField : BlazeFusionView
{
    public string Title { get; set; };
}
```

```razor
<!-- DisplayField.cshtml -->

@model DisplayField

<div class="display-field">
  <div class="display-field-title">
    @Model.Title
  </div>

  <div class="display-field-content">
    @Model.Slot()
  </div>
</div>
```

Usage:

```razor
<!-- SomePage.cshtml -->

<display-field title="Price">
  <i>199</i> EUR
</display-field>
```

Remarks:
- `Model.Slot()` renders the child content passed by the tag caller


## Slots

Slots are placeholders for html content inside BlazeFusion views that can be passed by the caller. Here is an example of a `Card` tag:

```c#
// Card.cshtml.cs

public class Card : BlazeView;
```

```razor
<!-- Card.cshtml -->

@model Card

<div class="card">
  <div class="card-header">
    @Model.Slot("header")
  </div>

  <div class="card-content">
    @Model.Slot()
  </div>

  <div class="card-footer">
    @Model.Slot("footer")
  </div>
</div>
```

Usage:

```razor
<!-- SomePage.cshtml -->

<card>
  <slot name=“header”>
    <strong>Laptop</strong>
  </slot>

  Information about the product

  <slot name=“footer”>
    <i>Price: $199</i>
  </slot>
</card>
```

Remarks:
- `Model.Slot("header")` renders the content of passed through `<slot name=“header”>`
- `Model.Slot("footer")` renders the content of passed through `<slot name=“footer”>`
- `Model.Slot()` renders the rest of the child content

## Differences between BlazeFusion components and BlazeFusion views

**BlazeFusion views:**
- Used to render views.
- Not stateful or interactive.
- Replacement for partial views, editors or regular view components.
- Rerendered on each request.

**BlazeFusion components:**
- Used to render functional components.
- Stateful and interactive.
- Should be used when state is needed.
- Rerendered only in specific situations, like action calls or events.
