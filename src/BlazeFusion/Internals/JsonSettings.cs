using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazeFusion;
internal static class JsonSettings
{
    public static readonly JsonSerializerOptions SerializerSettings = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}
