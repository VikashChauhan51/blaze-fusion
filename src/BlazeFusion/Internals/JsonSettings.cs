using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazeFusion;

/// <summary>
/// Provides default JSON serializer settings for the application.
/// </summary>
internal static class JsonSettings
{
    /// <summary>
    /// Gets the default JSON serializer settings.
    /// </summary>
    public static readonly JsonSerializerOptions DefaultSerializerSettings = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    /// <summary>
    /// Gets the JSON serializer settings with reference handling to ignore cycles.
    /// </summary>
    public static readonly JsonSerializerOptions JsonSerializerSettings = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
    };
}
