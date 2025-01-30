using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BlazeFusion;
 
/// <summary>
/// Provides metadata for models in a Blazor application.
/// </summary>
internal sealed class BlazeModelMetadataProvider : DefaultModelMetadataProvider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BlazeModelMetadataProvider"/> class.
    /// </summary>
    /// <param name="detailsProvider">The provider of metadata details.</param>
    public BlazeModelMetadataProvider(ICompositeMetadataDetailsProvider detailsProvider) : base(detailsProvider)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BlazeModelMetadataProvider"/> class.
    /// </summary>
    /// <param name="detailsProvider">The provider of metadata details.</param>
    /// <param name="optionsAccessor">The accessor for MVC options.</param>
    public BlazeModelMetadataProvider(ICompositeMetadataDetailsProvider detailsProvider, IOptions<MvcOptions> optionsAccessor) : base(detailsProvider, optionsAccessor)
    {
    }

    /// <summary>
    /// Creates property metadata details for the specified model metadata identity.
    /// </summary>
    /// <param name="key">The model metadata identity.</param>
    /// <returns>An array of <see cref="DefaultMetadataDetails"/>.</returns>
    protected override DefaultMetadataDetails[] CreatePropertyDetails(ModelMetadataIdentity key) =>
        base.CreatePropertyDetails(key)
            .Where(d => d.Key.PropertyInfo?.DeclaringType != typeof(ViewComponent))
            .ToArray();
}
