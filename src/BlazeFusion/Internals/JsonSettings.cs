using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazeFusion;
internal static class JsonSettings
{
    public static readonly JsonSerializerOptions DefaultSerializerSettings = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public static readonly JsonSerializerOptions JsonSerializerSettings = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
    };
}
