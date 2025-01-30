using System.Collections.Concurrent;
using System.Reflection;

namespace BlazeFusion;

/// <summary>
/// Provides methods to extract properties from objects and cache them for performance.
/// </summary>
internal static class PropertyExtractor
{
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> PropertiesCache = new();

    /// <summary>
    /// Gets the properties of the specified object as a dictionary.
    /// </summary>
    /// <param name="source">The object from which to extract properties.</param>
    /// <returns>A dictionary containing the property names and values of the object.</returns>
    public static Dictionary<string, object> GetPropertiesFromObject(object source) =>
        source == null
            ? new()
            : PropertiesCache.GetOrAdd(source.GetType(), type => type.GetProperties())
                .ToDictionary(p => p.Name, p => p.GetValue(source));
}
