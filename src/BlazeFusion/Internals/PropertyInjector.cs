using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Json;

namespace BlazeFusion;

/// <summary>
/// Provides methods for injecting and manipulating properties of objects.
/// </summary>
internal static class PropertyInjector
{
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> CachedPropertyInfos = new();

    /// <summary>
    /// Serializes the declared properties of the specified type and instance.
    /// </summary>
    /// <param name="type">The type of the object.</param>
    /// <param name="instance">The instance of the object.</param>
    /// <returns>A JSON string representing the serialized properties.</returns>
    public static string SerializeDeclaredProperties(Type type, object instance)
    {
        var regularProperties = GetRegularProperties(type, instance);
        return JsonSerializer.Serialize(regularProperties, JsonSettings.JsonSerializerSettings);
    }

    /// <summary>
    /// Gets the regular properties of the specified type and instance.
    /// </summary>
    /// <param name="type">The type of the object.</param>
    /// <param name="instance">The instance of the object.</param>
    /// <returns>A dictionary of property names and values.</returns>
    private static IDictionary<string, object> GetRegularProperties(Type type, object instance) =>
        GetPropertyInfos(type).ToDictionary(p => p.Name, p => p.GetValue(instance));

    /// <summary>
    /// Gets the property information for the specified type.
    /// </summary>
    /// <param name="type">The type of the object.</param>
    /// <returns>An enumerable of property information.</returns>
    private static IEnumerable<PropertyInfo> GetPropertyInfos(Type type)
    {
        if (CachedPropertyInfos.TryGetValue(type, out var properties))
        {
            return properties;
        }

        var viewComponentType = typeof(TagHelper);
        var blazeComponentType = typeof(BlazeComponent);

        var baseProps = new[] { "Key", "IsModelTouched", "TouchedProperties" };

        var propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(p => (baseProps.Contains(p.Name) && p.DeclaringType == blazeComponentType)
                        || (p.DeclaringType != viewComponentType && p.DeclaringType != blazeComponentType
                            && p.GetGetMethod()?.IsPublic == true
                            && p.GetSetMethod()?.IsPublic == true
                            && !p.GetCustomAttributes<TransientAttribute>().Any())
            )
            .ToArray();

        CachedPropertyInfos.TryAdd(type, propertyInfos);

        return propertyInfos;
    }

    /// <summary>
    /// Sets the value of the specified property path on the target object.
    /// </summary>
    /// <param name="target">The target object.</param>
    /// <param name="propertyPath">The property path.</param>
    /// <param name="value">The value to set.</param>
    /// <exception cref="ArgumentNullException">Thrown when the target is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the property path is empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the property path is invalid.</exception>
    /// <exception cref="NotSupportedException">Thrown when the property path contains an unsupported indexer.</exception>
    public static void SetPropertyValue(object target, string propertyPath, object value)
    {
        if (target == null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        if (string.IsNullOrWhiteSpace(propertyPath))
        {
            throw new ArgumentException("Property path cannot be empty.", nameof(propertyPath));
        }

        var properties = propertyPath.Split('.');
        var currentObject = target;

        for (var i = 0; i < properties.Length - 1; i++)
        {
            currentObject = GetObjectOrIndexedValue(currentObject, properties[i]);
        }

        if (currentObject == null)
        {
            return;
        }

        var propName = properties[^1];

        if (string.IsNullOrWhiteSpace(propName))
        {
            throw new InvalidOperationException("Wrong property path");
        }

        if (propName.Contains('['))
        {
            throw new NotSupportedException();
        }

        var propertyInfo = currentObject.GetType().GetProperty(propName);
        if (propertyInfo == null)
        {
            return;
        }

        var converter = TypeDescriptor.GetConverter(propertyInfo.PropertyType);
        var convertedValue = value == null || value.GetType() == propertyInfo.PropertyType
            ? value
            : converter.ConvertFrom(value);

        propertyInfo.SetValue(currentObject, convertedValue);
    }

    /// <summary>
    /// Gets the property setter for the specified property path on the target object.
    /// </summary>
    /// <param name="target">The target object.</param>
    /// <param name="propertyPath">The property path.</param>
    /// <param name="value">The value to set.</param>
    /// <returns>A tuple containing the value, setter action, and property type, or null if the property is not found.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the target is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the property path is empty.</exception>
    public static (object Value, Action<object> Setter, Type PropertyType)? GetPropertySetter(object target, string propertyPath, object value)
    {
        if (target == null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        if (string.IsNullOrWhiteSpace(propertyPath))
        {
            throw new ArgumentException("Property path cannot be empty.", nameof(propertyPath));
        }

        var properties = propertyPath.Split('.');
        var currentObject = target;

        for (var i = 0; i < properties.Length - 1; i++)
        {
            currentObject = GetObjectOrIndexedValue(currentObject, properties[i]);
        }

        return SetValueOnObject(currentObject, properties[^1], value);
    }

    /// <summary>
    /// Gets the value of the specified property or indexed property on the object.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="propName">The property name.</param>
    /// <returns>The value of the property or indexed property.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the property path is invalid.</exception>
    private static object GetObjectOrIndexedValue(object obj, string propName)
    {
        if (obj == null)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(propName))
        {
            throw new InvalidOperationException("Wrong property path");
        }

        return propName.Contains('[')
            ? GetIndexedValue(obj, propName)
            : obj.GetType().GetProperty(propName)?.GetValue(obj);
    }

    /// <summary>
    /// Gets the value of the specified indexed property on the object.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="propName">The indexed property name.</param>
    /// <returns>The value of the indexed property.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the value type is incorrect.</exception>
    private static object GetIndexedValue(object obj, string propName)
    {
        if (obj == null)
        {
            return null;
        }

        var (index, cleanedPropName) = GetIndexAndCleanedPropertyName(propName);
        var propertyInfo = obj.GetType().GetProperty(cleanedPropName);

        if (propertyInfo == null)
        {
            return null;
        }

        var value = propertyInfo.GetValue(obj);

        if (value == null)
        {
            return null;
        }

        if (propertyInfo.PropertyType.IsArray)
        {
            return value is Array array && index < array.Length
                ? array.GetValue(index)
                : throw new InvalidOperationException("Wrong value type");
        }

        if (typeof(IList).IsAssignableFrom(propertyInfo.PropertyType))
        {
            return value is IList list && index < list.Count
                ? list[index]
                : throw new InvalidOperationException("Wrong value type");
        }

        throw new InvalidOperationException("Wrong indexer property");
    }

    /// <summary>
    /// Gets the index and cleaned property name from the specified indexed property name.
    /// </summary>
    /// <param name="propName">The indexed property name.</param>
    /// <returns>A tuple containing the index and cleaned property name.</returns>
    private static (int, string) GetIndexAndCleanedPropertyName(string propName)
    {
        var iteratorStart = propName.IndexOf('[');
        var iteratorEnd = propName.IndexOf(']');
        var iteratorValue = propName.Substring(iteratorStart + 1, iteratorEnd - iteratorStart - 1);
        var cleanedPropName = propName[..iteratorStart];
        return (Convert.ToInt32(iteratorValue), cleanedPropName);
    }

    /// <summary>
    /// Sets the value of the specified property on the object.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="propName">The property name.</param>
    /// <param name="valueToSet">The value to set.</param>
    /// <returns>A tuple containing the value, setter action, and property type, or null if the property is not found.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the property path is invalid.</exception>
    private static (object, Action<object>, Type)? SetValueOnObject(object obj, string propName, object valueToSet)
    {
        if (obj == null)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(propName))
        {
            throw new InvalidOperationException("Wrong property path");
        }

        if (propName.Contains('['))
        {
            return SetIndexedValue(obj, propName, valueToSet);
        }

        var propertyInfo = obj.GetType().GetProperty(propName);
        if (propertyInfo == null)
        {
            return null;
        }

        var convertedValue = ConvertValue(valueToSet, propertyInfo.PropertyType);
        propertyInfo.SetValue(obj, convertedValue);
        return (convertedValue, val => propertyInfo.SetValue(obj, val), propertyInfo.PropertyType);
    }

    /// <summary>
    /// Sets the value of the specified indexed property on the object.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="propName">The indexed property name.</param>
    /// <param name="valueToSet">The value to set.</param>
    /// <returns>A tuple containing the value, setter action, and property type, or null if the property is not found.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the value type is incorrect or indexed access is not supported.</exception>
    private static (object, Action<object>, Type)? SetIndexedValue(object obj, string propName, object valueToSet)
    {
        var (index, cleanedPropName) = GetIndexAndCleanedPropertyName(propName);
        var propertyInfo = obj.GetType().GetProperty(cleanedPropName);
        var convertedValue = ConvertValue(valueToSet, propertyInfo!.PropertyType);

        var value = propertyInfo.GetValue(obj);

        if (value == null)
        {
            throw new InvalidOperationException("Cannot set value to null");
        }

        if (propertyInfo.PropertyType.IsArray)
        {
            if (value is not Array array)
            {
                throw new InvalidOperationException("Wrong type");
            }

            return (convertedValue, val => array.SetValue(val, index), propertyInfo!.PropertyType);
        }

        if (typeof(IList).IsAssignableFrom(propertyInfo.PropertyType))
        {
            if (value is not IList list)
            {
                throw new InvalidOperationException("Wrong type");
            }

            return (convertedValue, val => list[index] = val, propertyInfo!.PropertyType);
        }

        throw new InvalidOperationException($"Indexed access for property '{cleanedPropName}' is not supported.");
    }

    /// <summary>
    /// Converts the specified value to the destination type.
    /// </summary>
    /// <param name="valueToConvert">The value to convert.</param>
    /// <param name="destinationType">The destination type.</param>
    /// <returns>The converted value.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the conversion is not supported.</exception>
    private static object ConvertValue(object valueToConvert, Type destinationType)
    {
        if (valueToConvert is not StringValues stringValues)
        {
            return valueToConvert;
        }

        if (typeof(IFormFile).IsAssignableFrom(destinationType) && StringValues.IsNullOrEmpty(stringValues))
        {
            return null;
        }

        if (typeof(IFormFile[]).IsAssignableFrom(destinationType) && StringValues.IsNullOrEmpty(stringValues))
        {
            return Array.Empty<IFormFile>();
        }

        if (typeof(IEnumerable<IFormFile>).IsAssignableFrom(destinationType) && StringValues.IsNullOrEmpty(stringValues))
        {
            return new List<IFormFile>();
        }

        var converter = TypeDescriptor.GetConverter(destinationType!);

        if (!converter.CanConvertFrom(typeof(string)))
        {
            throw new InvalidOperationException($"Cannot convert StringValues to '{destinationType}'.");
        }

        if (!destinationType.IsArray || stringValues is { Count: <= 1 })
        {
            try
            {
                return converter.ConvertFromString(valueToConvert.ToString());
            }
            catch
            {
                return Activator.CreateInstance(destinationType);
            }
        }

        var elementType = destinationType.GetElementType();
        var array = Array.CreateInstance(elementType, stringValues.Count);
        for (var i = 0; i < stringValues.Count; i++)
        {
            try
            {
                array.SetValue(converter.ConvertFromString(stringValues[i]), i);
            }
            catch
            {
                array.SetValue(Activator.CreateInstance(elementType), i);
            }
        }

        return array;
    }
}
