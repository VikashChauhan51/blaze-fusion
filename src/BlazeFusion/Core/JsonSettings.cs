using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using BlazeFusion.Converters;
namespace BlazeFusion;
internal static class JsonSettings
{
    public static readonly JsonSerializerSettings SerializerSettings = new()
    {
        Converters = new JsonConverter[] { new Int32Converter() }.ToList(),
        NullValueHandling = NullValueHandling.Ignore,
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
    };
}
