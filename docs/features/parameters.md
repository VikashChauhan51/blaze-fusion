---
outline: deep
---

# Parameters

Parameters are public properties on a component and are used to store component's state. They also allow the passing of
data or settings from a parent component to a child component. Parameters can include any types of values, such as
integers, strings, and complex objects.

Let's look at the Counter component:

```csharp
// Counter.cshtml.cs

public class Counter : BlazeComponent
{
    public int Count { get; set; }
}
```

The `Count` property can be set by a parameter using dash-case notation:

```razor
<!-- Index.cshtml -->

<counter count="10" />
```

or

```razor
<!-- Index.cshtml -->

<blaze name="Counter" params="@(new { Count = 10 })"/>
```

or

```razor
<!-- Index.cshtml -->

@await Html.Blaze("Counter", new { Count = 10 })
```

> NOTE: For multi-word properties, `Data` word cannot be used as the leading one, for example `DataSource`. `Database`
> is fine, since here `Data` is not a word, but just a part of the word.

## Transient properties

Sometimes there is no need to persist the property value across the request because its value is valid only within
the current request, for example a message after successful saving.

Another use case is handling the list of rows that you want to show in a table. If there are many rows and they should
be
fetched from the database on each parameter change, you probably don't want to keep the state of those rows across
requests.

There are two ways to define a transient property:

### `Transient` attribute

```csharp
public class ProductForm : BlazeComponent
{
    [Transient]
    public bool IsSuccess { get; set; }
}
```

### Property without setter

```csharp
public class ProductForm : BlazeComponent
{
    public DateTime CurrentDate => DateTime.Now;
}
```

## Hiding properties from a parameter list

By default, all the properties become available as parameters when rendering a component.
If you want to hide some of them, you can use the `HtmlAttributeNotBound` attribute:

```csharp
public class ProductForm : BlazeComponent
{
    public string Name { get; set; }

    [HtmlAttributeNotBound]
    public List<string> Currencies { get; set; }
}
```

Now you can pass only the Name, but Currencies will not be available:

```razor
@* Ok *@
<product-form name="Product 1" />

@* Currencies won't be passed *@
<product-form name="Product 1" currencies="@currencies" />
```

## State of the parameters in time

The values are passed to the component only once and any update to the parameters won't refresh the parent component. If
you want the component to refresh its state, you would have to use events or change the [key parameter](#key) of the
component.

## Key

When rendering a BlazeFusion component, you can provide an optional `key` argument:

```razor
<my-component key="1" />
```

or

```razor
<blaze name="MyComponent" key="1" />
```

It's used when you have multiple components of the same type on the page to make it possible to distinguish them during
DOM updates.

Usage examples:

```razor
@foreach (var item in Items)
{
  <product key="@item.Id"/>
}
```

or

```razor
<product key="1"/>
<product key="2"/>
```

You can also use `key` to force re-render of your component:

```razor
<items data="@Model.Items" key="@Model.Items.CalculateHashCode()" />
```

Where `CalculateHashCode` is an extension method returning unique hash code for the collection.
Now, whenever `Model.Items` changes, BlazeFusion will re-render the component `Items` and pass new parameter.

## Caching

Let's imagine you need to show list of customers in a table. It's good to use caching per request for such rows data,
because you might want to access your filtered or sorted list in your view and actions, and you don't want to fetch the
data each time you access it.
BlazeFusion has a solution for that which is built-in caching. To enable caching, create a read-only property that uses
`Cache` method.
If the property name is called `Customers`, you can get value either in view or component from the cache using
`Customers.Value`. Example:

```c#
// CustomerList.cshtml.cs

public class CustomerList(IDatabase database) : BlazeComponent
{
    public string SearchPhrase { get; set; }

    public HashSet<string> Selection { get; set; } = new();

    public Cache<Task<List<Customer>>> Customers => Cache(async () =>
    {
        var query = database.Query<Customer>();

        if (!string.IsNullOrWhiteSpace(SearchPhrase))
        {
            query = query.Where(p => p.Name.Contains(SearchPhrase));
        }

        return await query.ToListAsync();
    });

    public async Task Print()
    {
        var customers = await Customers.Value;

        if (!customers.Any())
        {
            return;
        }

        var customerIds = customers.Select(c => c.Id).ToList();
        Location(Url.Page("/Customers/Print"), new CustomersPrintPayload(customerIds));
    }
}
```

```razor
<!-- CustomerList.cshtml -->

@model CustomerList

<input asp-for="SearchPhrase" bind:input.debounce placeholder="Search..." />

<table class="table table-sm">
  <thead>
  <tr>
    <td>Customer name</td>
  </tr>
  </thead>
  <tbody>
  @foreach (var customer in await Model.Customers.Value)
  {
    <tr>
      <td>@customer.Name</td>
    </tr>
  }
  </tbody>
</table>
```
