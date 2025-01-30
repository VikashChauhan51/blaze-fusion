using System.Diagnostics;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BlazeFusion;


/// <summary>
/// Represents a dynamic view bag for storing and retrieving view data in a Blazor application.
/// </summary>
[DebuggerDisplay("Count = {ViewData.Count}")]
[DebuggerTypeProxy(typeof(DynamicViewDataView))]
internal sealed class ViewBag : DynamicObject
{
    private readonly Func<ViewDataDictionary> _viewDataFunc;

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewBag"/> class.
    /// </summary>
    /// <param name="viewDataFunc">A function that returns the <see cref="ViewDataDictionary"/>.</param>
    public ViewBag(Func<ViewDataDictionary> viewDataFunc) =>
        _viewDataFunc = viewDataFunc;

    /// <summary>
    /// Gets the view data dictionary.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the view data is invalid.</exception>
    private ViewDataDictionary ViewData =>
        _viewDataFunc() ?? throw new InvalidOperationException("Invalid view data");

    /// <summary>
    /// Returns the dynamic member names.
    /// </summary>
    /// <returns>An enumerable of dynamic member names.</returns>
    public override IEnumerable<string> GetDynamicMemberNames() =>
        ViewData.Keys;

    /// <summary>
    /// Tries to get the member with the specified name.
    /// </summary>
    /// <param name="binder">The binder that provides the name of the member to get.</param>
    /// <param name="result">When this method returns, contains the value of the member, if found; otherwise, null.</param>
    /// <returns>true if the member was found; otherwise, false.</returns>
    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
        result = ViewData[binder.Name];
        return true;
    }

    /// <summary>
    /// Tries to set the member with the specified name.
    /// </summary>
    /// <param name="binder">The binder that provides the name of the member to set.</param>
    /// <param name="value">The value to set to the member.</param>
    /// <returns>true if the member was set; otherwise, false.</returns>
    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
        ViewData[binder.Name] = value;
        return true;
    }

    /// <summary>
    /// Represents a debugger view for the dynamic view data.
    /// </summary>
    private sealed class DynamicViewDataView
    {
        private readonly ViewDataDictionary _viewData;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicViewDataView"/> class.
        /// </summary>
        /// <param name="dictionary">The <see cref="ViewBag"/> instance.</param>
        public DynamicViewDataView(ViewBag dictionary)
        {
            _viewData = dictionary.ViewData;
        }

        /// <summary>
        /// Gets the items in the view data dictionary.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public KeyValuePair<string, object>[] Items => _viewData.ToArray();
    }
}
