using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BlazeFusion;
 
internal class BlazeModelMetadataProvider : DefaultModelMetadataProvider
{
    public BlazeModelMetadataProvider(ICompositeMetadataDetailsProvider detailsProvider) : base(detailsProvider)
    {
    }

    public BlazeModelMetadataProvider(ICompositeMetadataDetailsProvider detailsProvider, IOptions<MvcOptions> optionsAccessor) : base(detailsProvider, optionsAccessor)
    {
    }

    protected override DefaultMetadataDetails[] CreatePropertyDetails(ModelMetadataIdentity key) =>
        base.CreatePropertyDetails(key)
            .Where(d => d.Key.PropertyInfo?.DeclaringType != typeof(ViewComponent))
            .ToArray();
}
